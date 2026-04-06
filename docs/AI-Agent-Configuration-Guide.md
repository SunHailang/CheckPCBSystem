# AI Agent 配置指南

> 本文档介绍如何在 CheckPCBSystem（PCB 缺陷检测系统）中配置和使用 AI Agent（YOLO 目标检测模型），以实现自动化 PCB 缺陷检测。

---

## 目录

1. [系统概述](#1-系统概述)
2. [环境准备](#2-环境准备)
3. [AI 模型配置](#3-ai-模型配置)
4. [检测类别说明](#4-检测类别说明)
5. [推理流程配置](#5-推理流程配置)
6. [输出结果格式](#6-输出结果格式)
7. [自定义模型集成](#7-自定义模型集成)
8. [常见问题排查](#8-常见问题排查)

---

## 1. 系统概述

CheckPCBSystem 是一个基于 Windows Forms 的 PCB 缺陷检测桌面应用程序。系统通过集成 YOLO 系列目标检测模型，对 PCB 图像进行自动化缺陷识别与分类。

### 系统架构

```
┌─────────────────────────────────────────────────────┐
│                  CheckPCBSystem                     │
│                                                     │
│  ┌──────────┐   ┌──────────────┐   ┌─────────────┐ │
│  │  UI 层   │──▶│  业务逻辑层   │──▶│  数据持久层  │ │
│  │ WinForms │   │ 模型调度/解析 │   │ MySQL/文件  │ │
│  └──────────┘   └──────┬───────┘   └─────────────┘ │
│                        │                            │
│                        ▼                            │
│               ┌────────────────┐                    │
│               │  AI Agent 层   │                    │
│               │  YOLO 推理引擎  │                    │
│               └────────────────┘                    │
└─────────────────────────────────────────────────────┘
```

### 核心组件

| 组件 | 说明 |
|------|------|
| **MainWindow** | 主检测界面，负责图像选择、模型选择和结果展示 |
| **YOLO 推理引擎** | 外部 Python 推理进程，执行目标检测 |
| **ResultData** | 检测结果数据模型，包含缺陷类型、位置、置信度 |
| **MySqlData** | 数据库连接层，管理用户认证 |
| **GetVideo.py** | Python 摄像头采集脚本，支持实时视频输入 |

---

## 2. 环境准备

### 2.1 硬件要求

| 项目 | 最低要求 | 推荐配置 |
|------|---------|---------|
| CPU | Intel i5 / AMD Ryzen 5 | Intel i7 / AMD Ryzen 7 或更高 |
| 内存 | 8 GB | 16 GB 或更高 |
| GPU | 支持 CUDA 的 NVIDIA 显卡 | NVIDIA RTX 3060 或更高 |
| 存储 | 10 GB 可用空间 | SSD，50 GB 可用空间 |
| 摄像头 | USB 摄像头（可选） | 工业级高分辨率摄像头 |

### 2.2 软件依赖

#### .NET 运行时

- .NET Framework 4.7.2 或更高版本
- Visual Studio 2019 或更高版本（开发环境）

#### Python 环境

```bash
# 安装 Python 3.8+
python --version

# 创建虚拟环境（推荐）
python -m venv pcb_env
pcb_env\Scripts\activate     # Windows
source pcb_env/bin/activate  # Linux/Mac

# 安装核心依赖
pip install ultralytics       # YOLOv8 框架
pip install torch torchvision # PyTorch 深度学习框架
pip install opencv-python     # 图像处理
pip install numpy             # 数值计算
```

#### 数据库

- MySQL 8.0 或更高版本
- 创建数据库和用户表：

```sql
CREATE DATABASE IF NOT EXISTS YoloCheckRoad;
USE YoloCheckRoad;

CREATE TABLE IF NOT EXISTS UserTable (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(20) NOT NULL,
    password VARCHAR(20) NOT NULL
);
```

### 2.3 目录结构配置

系统依赖以下目录结构进行模型推理和结果存储：

```
<工作目录>/
├── resource/          # 待检测的原始 PCB 图像
├── detect/            # 检测后标注了缺陷框的图像
├── text/              # 检测结果的文本文件（每张图像对应一个 .txt）
└── models/            # YOLO 模型权重文件（.pt）
    ├── YOLOv8-EBS.pt
    ├── YOLOv8m.pt
    ├── YOLOv7.pt
    └── YOLOv5.pt
```

> **注意：** 请确保应用程序对上述目录拥有读写权限。

---

## 3. AI 模型配置

### 3.1 支持的模型

系统内置支持以下 YOLO 模型变体：

| 模型 | 文件名 | 特点 | 推荐场景 |
|------|--------|------|---------|
| **YOLOv8-EBS** | `YOLOv8-EBS.pt` | 增强版 YOLOv8，针对 PCB 缺陷优化 | 高精度生产环境 |
| **YOLOv8m** | `YOLOv8m.pt` | YOLOv8 中等规模，精度与速度平衡 | 通用检测场景 |
| **YOLOv7** | `YOLOv7.pt` | 上一代模型，兼容性好 | 低配置设备 |
| **YOLOv5** | `YOLOv5.pt` | 经典模型，轻量高效 | 快速原型验证 |

### 3.2 模型文件放置

将下载或训练好的模型权重文件（`.pt`）放置到指定的模型目录：

```bash
# 示例：复制模型文件
cp YOLOv8-EBS.pt <工作目录>/models/
```

### 3.3 在界面中选择模型

1. 启动 CheckPCBSystem 并登录
2. 在主窗口（MainWindow）中找到 **"模型选择"** 下拉框
3. 从列表中选择要使用的 YOLO 模型
4. 选定后系统会使用该模型进行后续的缺陷检测

### 3.4 模型参数调优

可以通过修改推理脚本中的参数来优化检测效果：

```python
from ultralytics import YOLO

# 加载模型
model = YOLO("YOLOv8-EBS.pt")

# 推理参数配置
results = model.predict(
    source="resource/pcb_image.jpg",  # 输入图像路径
    conf=0.25,                         # 置信度阈值（0.0 - 1.0）
    iou=0.45,                          # NMS IoU 阈值
    imgsz=640,                         # 输入图像尺寸
    device="0",                        # GPU 设备编号（"cpu" 使用 CPU）
    save=True,                         # 保存标注图像
    save_txt=True,                     # 保存文本结果
    project="detect",                  # 输出项目目录
    name="results",                    # 输出子目录名称
)
```

#### 关键参数说明

| 参数 | 默认值 | 说明 |
|------|--------|------|
| `conf` | 0.25 | 置信度阈值，值越高漏检越多但误检越少 |
| `iou` | 0.45 | NMS（非极大值抑制）阈值，用于去除重叠检测框 |
| `imgsz` | 640 | 输入图像缩放尺寸，越大精度越高但速度越慢 |
| `device` | `"0"` | 推理设备，`"0"` 为第一块 GPU，`"cpu"` 为 CPU 推理 |
| `half` | `False` | 是否使用 FP16 半精度推理，可加速但可能轻微降低精度 |
| `max_det` | 300 | 每张图像最大检测数量 |
| `augment` | `False` | 是否启用测试时增强（TTA），提升精度但降低速度 |

---

## 4. 检测类别说明

系统当前配置了以下缺陷检测类别：

| 类别 ID | 类别名称 | 英文描述 | 说明 |
|---------|---------|---------|------|
| 0 | 纵向裂缝 | Longitudinal Crack | 沿 PCB 纵向方向的裂缝缺陷 |
| 1 | 横向裂缝 | Transverse Crack | 沿 PCB 横向方向的裂缝缺陷 |
| 2 | 龟裂裂缝 | Alligator Crack | 网状分布的龟裂缺陷 |
| 3 | 坑洞 | Pothole | PCB 表面的坑洞缺陷 |
| 4 | 修补 | Patch | 已修补的区域标记 |
| 5 | 未知 | Unknown | 无法归类的未知缺陷 |

### 自定义检测类别

如需修改或扩展检测类别，请按以下步骤操作：

1. **更新模型训练数据集** —— 在 YOLO 数据集配置中添加新类别
2. **重新训练模型** —— 使用包含新类别的数据集训练模型
3. **更新 C# 代码中的类别映射** —— 修改 `MainWindow.cs` 中的 `RoadDefectList` 数组：

```csharp
// MainWindow.cs 中的类别列表
private string[] RoadDefectList = new string[]
{
    "纵向裂缝",  // 类别 0
    "横向裂缝",  // 类别 1
    "龟裂裂缝",  // 类别 2
    "坑洞",      // 类别 3
    "修补",      // 类别 4
    "未知",      // 类别 5
    "新类别",    // 类别 6（自定义扩展）
};
```

---

## 5. 推理流程配置

### 5.1 图像检测流程

```
用户选择图像 ──▶ 加载到 resource/ ──▶ 选择 YOLO 模型
                                          │
                                          ▼
                                    执行模型推理
                                          │
                        ┌─────────────────┼─────────────────┐
                        ▼                 ▼                 ▼
                   detect/ 目录      text/ 目录        UI 结果展示
                  （标注图像）     （检测结果文本）    （DataGridView）
```

### 5.2 推理脚本配置

创建或配置推理脚本 `predict.py`：

```python
#!/usr/bin/env python3
"""PCB 缺陷检测推理脚本"""

import sys
import os
from pathlib import Path
from ultralytics import YOLO


def predict(image_path: str, model_path: str, output_dir: str):
    """
    执行 PCB 缺陷检测推理。

    Args:
        image_path: 待检测图像路径
        model_path: YOLO 模型权重路径
        output_dir: 输出结果目录
    """
    # 加载模型
    model = YOLO(model_path)

    # 执行推理
    results = model.predict(
        source=image_path,
        conf=0.25,
        iou=0.45,
        imgsz=640,
        save=True,
        save_txt=True,
        project=output_dir,
        name="results",
    )

    # 处理并输出结果
    for result in results:
        boxes = result.boxes
        for box in boxes:
            cls_id = int(box.cls[0])
            confidence = float(box.conf[0])
            x1, y1, x2, y2 = box.xyxy[0].tolist()
            print(f"{cls_id} {confidence:.4f} {x1:.1f} {y1:.1f} {x2:.1f} {y2:.1f}")


if __name__ == "__main__":
    if len(sys.argv) < 3:
        print("用法: python predict.py <图像路径> <模型路径> [输出目录]")
        sys.exit(1)

    image = sys.argv[1]
    model = sys.argv[2]
    output = sys.argv[3] if len(sys.argv) > 3 else "output"

    predict(image, model, output)
```

### 5.3 摄像头实时检测配置

系统支持通过摄像头进行实时 PCB 检测：

1. **连接摄像头** —— 将 USB 摄像头或工业相机连接到计算机
2. **设备识别** —— 系统通过 AForge.NET 的 DirectShow 自动识别可用摄像头设备
3. **启动采集** —— 在主界面点击"开始检测"按钮启动视频流
4. **实时推理** —— 系统逐帧或按间隔对采集画面执行 YOLO 推理

#### GetVideo.py 配置

```python
import cv2

# 摄像头索引（0 为默认摄像头）
CAMERA_INDEX = 0

# 分辨率设置
FRAME_WIDTH = 1920
FRAME_HEIGHT = 1080

cap = cv2.VideoCapture(CAMERA_INDEX)
cap.set(cv2.CAP_PROP_FRAME_WIDTH, FRAME_WIDTH)
cap.set(cv2.CAP_PROP_FRAME_HEIGHT, FRAME_HEIGHT)

while cap.isOpened():
    ret, frame = cap.read()
    if not ret:
        break
    cv2.imshow("PCB Camera", frame)
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

cap.release()
cv2.destroyAllWindows()
```

---

## 6. 输出结果格式

### 6.1 文本结果格式

每张图像的检测结果保存为同名的 `.txt` 文件，每行表示一个检测到的缺陷：

```
<类别ID> <置信度> <x1> <y1> <x2> <y2>
```

#### 字段说明

| 字段 | 类型 | 说明 |
|------|------|------|
| 类别ID | int | 缺陷类别编号（0-5），对应检测类别表 |
| 置信度 | float | 模型对该检测结果的置信度（0.0 - 1.0） |
| x1, y1 | float | 缺陷边界框左上角坐标（像素） |
| x2, y2 | float | 缺陷边界框右下角坐标（像素） |

#### 示例

```
0 0.9234 120.5 45.2 280.3 190.7
2 0.8567 350.0 200.1 510.8 380.4
3 0.7891 50.2 300.0 150.6 420.3
```

### 6.2 UI 展示结果

检测结果在 MainWindow 的 DataGridView 中以表格形式展示：

| 列 | 说明 |
|----|------|
| 序号 | 缺陷检测结果的索引编号 |
| 检测结果 | 缺陷类别名称（如"纵向裂缝"） |
| 缺陷位置 | 缺陷在图像上的位置坐标 |
| 置信等级 | 模型检测的置信度评分 |

### 6.3 历史记录

每个用户的检测历史保存为 `{用户名}_history.txt` 文件：

```
<文件名> <文件目录路径>
```

---

## 7. 自定义模型集成

### 7.1 训练自定义模型

#### 准备数据集

创建符合 YOLO 格式的数据集配置文件 `pcb_dataset.yaml`：

```yaml
# PCB 缺陷检测数据集配置
path: /path/to/pcb_dataset     # 数据集根目录
train: images/train             # 训练图像目录
val: images/val                 # 验证图像目录
test: images/test               # 测试图像目录（可选）

# 类别数量
nc: 6

# 类别名称
names:
  0: longitudinal_crack   # 纵向裂缝
  1: transverse_crack     # 横向裂缝
  2: alligator_crack      # 龟裂裂缝
  3: pothole              # 坑洞
  4: patch                # 修补
  5: unknown              # 未知
```

#### 数据集目录结构

```
pcb_dataset/
├── images/
│   ├── train/            # 训练图像
│   │   ├── pcb_001.jpg
│   │   ├── pcb_002.jpg
│   │   └── ...
│   ├── val/              # 验证图像
│   │   ├── pcb_100.jpg
│   │   └── ...
│   └── test/             # 测试图像（可选）
│       └── ...
├── labels/
│   ├── train/            # 训练标注（YOLO 格式）
│   │   ├── pcb_001.txt
│   │   ├── pcb_002.txt
│   │   └── ...
│   ├── val/              # 验证标注
│   │   └── ...
│   └── test/             # 测试标注（可选）
│       └── ...
└── pcb_dataset.yaml      # 数据集配置文件
```

#### 标注格式

每行标注格式为 YOLO 归一化坐标格式：

```
<类别ID> <中心x> <中心y> <宽度> <高度>
```

所有坐标值归一化到 `[0, 1]` 范围（相对于图像尺寸）。

#### 执行训练

```python
from ultralytics import YOLO

# 基于预训练模型进行微调
model = YOLO("yolov8m.pt")

# 开始训练
results = model.train(
    data="pcb_dataset.yaml",    # 数据集配置
    epochs=100,                  # 训练轮数
    imgsz=640,                   # 训练图像尺寸
    batch=16,                    # 批量大小
    device="0",                  # GPU 设备
    workers=8,                   # 数据加载线程数
    patience=20,                 # 早停耐心值
    save=True,                   # 保存检查点
    project="runs/train",        # 训练输出目录
    name="pcb_yolov8",           # 实验名称
)
```

也可以通过命令行执行：

```bash
yolo detect train \
    data=pcb_dataset.yaml \
    model=yolov8m.pt \
    epochs=100 \
    imgsz=640 \
    batch=16 \
    device=0
```

### 7.2 模型评估

训练完成后，评估模型性能：

```python
from ultralytics import YOLO

model = YOLO("runs/train/pcb_yolov8/weights/best.pt")

# 在验证集上评估
metrics = model.val(data="pcb_dataset.yaml")

print(f"mAP50:    {metrics.box.map50:.4f}")
print(f"mAP50-95: {metrics.box.map:.4f}")
print(f"精确率:    {metrics.box.mp:.4f}")
print(f"召回率:    {metrics.box.mr:.4f}")
```

### 7.3 导出与部署

将训练好的模型部署到系统中：

```bash
# 1. 复制最佳模型权重到系统模型目录
cp runs/train/pcb_yolov8/weights/best.pt <工作目录>/models/Custom-PCB.pt

# 2. 在 MainWindow.cs 的模型下拉框中添加新模型选项
```

如需更高推理性能，可将模型导出为 ONNX 或 TensorRT 格式：

```python
from ultralytics import YOLO

model = YOLO("best.pt")

# 导出为 ONNX 格式
model.export(format="onnx", imgsz=640, simplify=True)

# 导出为 TensorRT 格式（需要 NVIDIA GPU）
model.export(format="engine", imgsz=640, half=True)
```

---

## 8. 常见问题排查

### Q1: 模型加载失败

**现象：** 选择模型后无法执行检测，报错模型文件不存在。

**解决方案：**
- 确认模型文件（`.pt`）已放置在正确目录
- 检查文件名是否与代码中配置的名称一致（区分大小写）
- 确认 Python 环境已正确安装 `ultralytics` 包

### Q2: GPU 推理不可用

**现象：** 推理速度极慢或报错 CUDA 不可用。

**解决方案：**
```bash
# 检查 CUDA 是否可用
python -c "import torch; print(torch.cuda.is_available())"

# 检查 CUDA 版本
nvidia-smi

# 安装匹配 CUDA 版本的 PyTorch
pip install torch torchvision --index-url https://download.pytorch.org/whl/cu118
```

### Q3: 检测精度不理想

**解决方案：**
- 降低置信度阈值 `conf`（如从 0.25 降到 0.15）以减少漏检
- 提升图像输入尺寸 `imgsz`（如从 640 改为 1280）
- 使用更大的模型（如从 YOLOv8m 切换到 YOLOv8-EBS）
- 收集更多样本重新训练模型

### Q4: 摄像头无法识别

**现象：** 启动视频采集时无可用设备。

**解决方案：**
- 确认摄像头已正确连接并被系统识别
- 检查摄像头驱动是否已安装
- 确认没有其他程序占用摄像头
- 尝试更换 USB 端口或摄像头索引

### Q5: 数据库连接失败

**现象：** 登录时提示无法连接数据库。

**解决方案：**
- 确认 MySQL 服务正在运行
- 检查 `App.config` 或 `MySqlData.cs` 中的连接字符串
- 确认数据库 `YoloCheckRoad` 和 `UserTable` 已创建
- 检查防火墙是否阻止了 3306 端口

### Q6: 检测结果为空

**现象：** 执行检测后 `text/` 目录下的结果文件为空。

**解决方案：**
- 确认输入图像质量良好（无过度模糊或曝光不当）
- 检查置信度阈值是否设置过高
- 验证模型是否针对当前类型的 PCB 进行过训练
- 查看推理日志输出，确认推理过程正常完成

---

## 附录

### A. 推荐的训练超参数

| 参数 | 推荐值 | 说明 |
|------|--------|------|
| epochs | 100-300 | 训练轮数，小数据集可适当增大 |
| imgsz | 640 | 标准训练尺寸，高精度可设 1280 |
| batch | 16 | 批量大小，根据显存调整 |
| lr0 | 0.01 | 初始学习率 |
| lrf | 0.01 | 最终学习率（lr0 的倍数） |
| momentum | 0.937 | SGD 动量 |
| weight_decay | 0.0005 | 权重衰减 |
| warmup_epochs | 3.0 | 学习率预热轮数 |
| mosaic | 1.0 | 马赛克数据增强概率 |
| mixup | 0.0 | MixUp 数据增强概率 |

### B. 性能基准参考

在 NVIDIA RTX 3060 (12 GB) 上的推理性能参考：

| 模型 | 输入尺寸 | 推理时间 | mAP50 |
|------|---------|---------|-------|
| YOLOv5s | 640×640 | ~5 ms | ~85% |
| YOLOv7 | 640×640 | ~8 ms | ~88% |
| YOLOv8m | 640×640 | ~10 ms | ~90% |
| YOLOv8-EBS | 640×640 | ~12 ms | ~92% |

> 注：以上数据为参考值，实际性能取决于具体硬件环境和数据集。

### C. 相关资源

- [Ultralytics YOLOv8 官方文档](https://docs.ultralytics.com/)
- [AForge.NET 框架文档](http://www.aforgenet.com/framework/docs/)
- [OpenCV Python 教程](https://docs.opencv.org/4.x/d6/d00/tutorial_py_root.html)
- [MySQL Connector/NET 文档](https://dev.mysql.com/doc/connector-net/en/)
