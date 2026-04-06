# AI Agent 配置文档

## 目录

- [1. 概述](#1-概述)
- [2. 系统架构](#2-系统架构)
- [3. 环境准备](#3-环境准备)
  - [3.1 Python 环境配置](#31-python-环境配置)
  - [3.2 YOLO 模型依赖](#32-yolo-模型依赖)
  - [3.3 数据库配置](#33-数据库配置)
- [4. AI 模型配置](#4-ai-模型配置)
  - [4.1 支持的模型](#41-支持的模型)
  - [4.2 模型文件放置](#42-模型文件放置)
  - [4.3 添加自定义模型](#43-添加自定义模型)
- [5. 检测 Agent 工作流程](#5-检测-agent-工作流程)
  - [5.1 数据流说明](#51-数据流说明)
  - [5.2 检测参数配置](#52-检测参数配置)
  - [5.3 检测结果格式](#53-检测结果格式)
- [6. 目录结构配置](#6-目录结构配置)
- [7. 缺陷分类配置](#7-缺陷分类配置)
- [8. 性能调优](#8-性能调优)
- [9. 故障排查](#9-故障排查)
- [10. 扩展指南](#10-扩展指南)

---

## 1. 概述

CheckPCBSystem 是一个基于 AI 视觉检测的缺陷检测系统，集成了多种 YOLO（You Only Look Once）目标检测模型，用于自动化识别和分析图像中的缺陷。本文档描述了系统中 AI Agent 的完整配置流程，包括环境搭建、模型部署、参数调优和扩展开发。

### 核心能力

| 能力 | 说明 |
|------|------|
| 多模型支持 | 同时支持 YOLOv5、YOLOv7、YOLOv8 及自定义变体 |
| 实时检测 | 支持图片和视频流的实时缺陷检测 |
| 多类别识别 | 支持 6 种缺陷类别的自动分类 |
| 历史追踪 | 检测结果自动归档，支持按用户追溯 |
| 置信度过滤 | 可配置的检测置信度阈值 |

---

## 2. 系统架构

```
┌──────────────────────────────────────────────────┐
│                 CheckPCBSystem                   │
│              (C# WinForms 客户端)                 │
│                                                  │
│  ┌────────────┐  ┌────────────┐  ┌────────────┐ │
│  │ 图片/视频   │  │  模型选择   │  │  结果展示   │ │
│  │   输入      │  │   面板      │  │   面板     │ │
│  └─────┬──────┘  └─────┬──────┘  └─────▲──────┘ │
│        │               │               │        │
│        ▼               ▼               │        │
│  ┌─────────────────────────────────────┤        │
│  │          文件系统通信层              │        │
│  │  resource/ ──→ detect/ + text/      │        │
│  └─────────────┬───────────────────────┘        │
│                │                                 │
└────────────────┼─────────────────────────────────┘
                 │
                 ▼
┌──────────────────────────────────────────────────┐
│              AI Detection Agent                  │
│            (Python YOLO Pipeline)                │
│                                                  │
│  ┌────────────┐  ┌────────────┐  ┌────────────┐ │
│  │  图像预处理 │  │  YOLO 推理  │  │  结果输出   │ │
│  │            │──▶│            │──▶│            │ │
│  └────────────┘  └────────────┘  └────────────┘ │
└──────────────────────────────────────────────────┘
```

**关键交互方式**：C# 前端与 Python AI Agent 之间通过 **文件系统** 进行通信：
- 输入目录 `resource/`：存放待检测的原始图片
- 输出目录 `detect/`：存放标注后的检测结果图片
- 元数据目录 `text/`：存放检测结果的文本描述文件

---

## 3. 环境准备

### 3.1 Python 环境配置

**推荐版本**：Python 3.8 - 3.11

```bash
# 创建虚拟环境（推荐）
python -m venv checkpcb_env

# 激活虚拟环境
# Windows:
checkpcb_env\Scripts\activate
# Linux/macOS:
source checkpcb_env/bin/activate

# 安装基础依赖
pip install -r requirements.txt
```

**手动安装核心依赖**：

```bash
# 计算机视觉基础
pip install opencv-python>=4.7.0
pip install numpy>=1.24.0
pip install Pillow>=9.5.0

# YOLO 框架（根据选择的模型版本安装）
# YOLOv5
pip install yolov5>=7.0

# YOLOv8（Ultralytics）
pip install ultralytics>=8.0.0

# YOLOv7（需从源码安装）
git clone https://github.com/WongKinYiu/yolov7.git
cd yolov7
pip install -r requirements.txt

# GPU 加速支持（可选但推荐）
pip install torch>=2.0.0 torchvision>=0.15.0 --index-url https://download.pytorch.org/whl/cu118
```

### 3.2 YOLO 模型依赖

| 依赖 | 最低版本 | 用途 |
|------|---------|------|
| `torch` | 2.0.0 | PyTorch 深度学习框架 |
| `torchvision` | 0.15.0 | 图像处理工具集 |
| `opencv-python` | 4.7.0 | 图像读取与处理 |
| `numpy` | 1.24.0 | 数值计算 |
| `ultralytics` | 8.0.0 | YOLOv8 框架 |
| `matplotlib` | 3.7.0 | 结果可视化 |
| `scipy` | 1.10.0 | 科学计算辅助 |

### 3.3 数据库配置

AI Agent 的检测结果会通过前端系统持久化到 MySQL 数据库。

```sql
-- 创建数据库
CREATE DATABASE IF NOT EXISTS YoloCheckRoad
  DEFAULT CHARACTER SET utf8mb4
  DEFAULT COLLATE utf8mb4_unicode_ci;

USE YoloCheckRoad;

-- 用户表
CREATE TABLE IF NOT EXISTS UserTable (
  id INT AUTO_INCREMENT PRIMARY KEY,
  name VARCHAR(20) NOT NULL UNIQUE,
  password VARCHAR(20) NOT NULL
);
```

**连接配置**（在 `MySqlData.cs` 中修改）：

| 参数 | 默认值 | 说明 |
|------|-------|------|
| Server | `localhost` | 数据库服务器地址 |
| Port | `3306` | 数据库端口 |
| Database | `YoloCheckRoad` | 数据库名称 |
| User | `root` | 数据库用户名 |
| Password | `123` | 数据库密码 |

> ⚠️ **安全提示**：生产环境中请务必更改默认数据库密码，并使用参数化查询防止 SQL 注入。

---

## 4. AI 模型配置

### 4.1 支持的模型

系统当前支持以下 YOLO 模型变体：

| 模型名称 | 文件名 | 特点 | 推荐场景 |
|---------|--------|------|---------|
| YOLOv8-EBS | `YOLOv8-EBS.pt` | 自定义增强版，针对缺陷检测优化 | **默认推荐**，精度最高 |
| YOLOv8m | `YOLOv8m.pt` | YOLOv8 中等规模模型 | 精度与速度平衡 |
| YOLOv7 | `YOLOv7.pt` | YOLOv7 标准模型 | 兼容旧项目 |
| YOLOv5 | `YOLOv5.pt` | YOLOv5 标准模型 | 轻量级部署 |

### 4.2 模型文件放置

将模型权重文件（`.pt` 格式）放置到指定目录：

```
项目根目录/
├── models/                    # 模型文件目录
│   ├── YOLOv8-EBS.pt         # 自定义增强模型
│   ├── YOLOv8m.pt            # YOLOv8 中等模型
│   ├── YOLOv7.pt             # YOLOv7 模型
│   └── YOLOv5.pt             # YOLOv5 模型
├── resource/                  # 待检测图片输入目录
├── detect/                    # 检测结果图片输出目录
└── text/                      # 检测元数据输出目录
```

### 4.3 添加自定义模型

要添加新的 YOLO 模型，需要完成以下步骤：

**步骤 1**：将训练好的 `.pt` 模型文件放入 `models/` 目录。

**步骤 2**：在 `MainWindow.cs` 中注册新模型。找到模型下拉框初始化代码，添加新模型条目：

```csharp
// MainWindow.cs - 模型下拉框初始化
comboBoxModel.Items.Add("YourCustomModel.pt");
```

**步骤 3**：确保 Python 检测脚本支持该模型的推理方式。如果使用 Ultralytics 框架的 YOLOv8 变体，通常无需额外修改；若使用其他框架，需在检测脚本中添加对应的推理逻辑。

**步骤 4**：验证模型输出格式与系统期望一致（见 [5.3 检测结果格式](#53-检测结果格式)）。

---

## 5. 检测 Agent 工作流程

### 5.1 数据流说明

AI Agent 的完整检测流程如下：

```
1. [用户操作] 在前端选择图片/视频帧或摄像头截图
         │
         ▼
2. [C# 前端] 将原始图片保存到 resource/ 目录
         │
         ▼
3. [触发检测] 定时器触发或手动触发 Python 检测脚本
         │
         ▼
4. [Python Agent] 读取 resource/ 中的图片
         │
         ▼
5. [模型推理] 使用选定的 YOLO 模型进行推理
         │
         ├──→ 生成标注图片 → 写入 detect/ 目录
         │
         └──→ 生成检测元数据 → 写入 text/ 目录
                  │
                  ▼
6. [C# 前端] 监控 detect/ 和 text/ 目录
         │
         ▼
7. [结果展示] 解析元数据，在 DataGrid 中显示结果
         │
         ▼
8. [历史记录] 将检测结果写入用户历史文件
```

### 5.2 检测参数配置

以下是 AI Agent 的核心检测参数：

| 参数 | 类型 | 默认值 | 说明 |
|------|------|-------|------|
| `model_path` | string | `YOLOv8-EBS.pt` | 模型文件路径 |
| `confidence_threshold` | float | `0.25` | 置信度阈值，低于此值的检测结果将被过滤 |
| `iou_threshold` | float | `0.45` | IoU（交并比）阈值，用于非极大值抑制 |
| `image_size` | int | `640` | 输入图像尺寸（像素） |
| `device` | string | `auto` | 推理设备：`cpu`、`cuda:0`、`auto` |
| `max_detections` | int | `300` | 单张图片最大检测数量 |
| `half_precision` | bool | `false` | 是否使用 FP16 半精度推理（需要 GPU） |

**Python 检测脚本配置示例**：

```python
from ultralytics import YOLO

# 加载模型
model = YOLO("models/YOLOv8-EBS.pt")

# 执行推理
results = model.predict(
    source="resource/input_image.jpg",   # 输入图片路径
    conf=0.25,                            # 置信度阈值
    iou=0.45,                             # IoU 阈值
    imgsz=640,                            # 输入尺寸
    device="auto",                        # 推理设备
    max_det=300,                          # 最大检测数
    half=False,                           # 半精度推理
    save=True,                            # 保存标注图片
    save_txt=True,                        # 保存检测元数据
    project="detect",                     # 输出目录
    name="results"                        # 子目录名
)
```

### 5.3 检测结果格式

**标注图片**（`detect/` 目录）：
- 格式：JPEG
- 内容：在原始图片上绘制检测框（bounding box）和类别标签
- 文件名：与原始图片同名

**元数据文件**（`text/` 目录）：
- 格式：纯文本（`.txt`）
- 文件名：与原始图片同名（扩展名改为 `.txt`）

每行一个检测结果，格式为：

```
<class_id> <x_start> <y_start> <x_end> <y_end> <confidence>
```

| 字段 | 类型 | 说明 |
|------|------|------|
| `class_id` | int | 缺陷类别 ID（0-5） |
| `x_start` | float | 检测框左上角 X 坐标 |
| `y_start` | float | 检测框左上角 Y 坐标 |
| `x_end` | float | 检测框右下角 X 坐标 |
| `y_end` | float | 检测框右下角 Y 坐标 |
| `confidence` | float | 置信度（0.0 - 1.0） |

**示例**：

```
0 120.5 80.3 350.2 200.7 0.92
3 400.0 150.0 520.5 280.3 0.85
```

---

## 6. 目录结构配置

系统依赖的核心目录结构及其作用：

```
项目根目录/
│
├── models/                          # AI 模型权重文件
│   ├── YOLOv8-EBS.pt
│   ├── YOLOv8m.pt
│   ├── YOLOv7.pt
│   └── YOLOv5.pt
│
├── resource/                        # 输入：原始图片/截图
│   ├── sample_001.jpg
│   └── sample_002.jpg
│
├── detect/                          # 输出：带标注的检测结果图片
│   ├── sample_001.jpg
│   └── sample_002.jpg
│
├── text/                            # 输出：检测元数据文本
│   ├── sample_001.txt
│   └── sample_002.txt
│
├── scripts/                         # Python 检测脚本
│   ├── detect.py                    # 主检测脚本
│   ├── batch_detect.py              # 批量检测脚本
│   └── utils.py                     # 工具函数
│
├── {UserName}_history.txt           # 用户检测历史记录
│
└── CheckPCBSystem/                  # C# 前端源码
    ├── MainWindow.cs
    ├── MySqlData.cs
    └── ...
```

> **重要**：确保 `resource/`、`detect/` 和 `text/` 目录存在且具有读写权限。如果目录不存在，系统无法正常完成检测流程。

---

## 7. 缺陷分类配置

系统当前支持 6 种缺陷分类。AI Agent 的输出类别 ID 对应关系如下：

| 类别 ID | 中文名称 | 英文名称 | 说明 |
|---------|---------|---------|------|
| 0 | 纵向裂缝 | Longitudinal Crack | 沿纵向延伸的裂缝 |
| 1 | 横向裂缝 | Transverse Crack | 沿横向延伸的裂缝 |
| 2 | 龟裂裂缝 | Alligator Crack | 网状交错裂缝 |
| 3 | 坑洞 | Pothole | 表面凹陷缺损 |
| 4 | 修补 | Patch | 已修补区域标记 |
| 5 | 未知 | Unknown | 无法归类的缺陷 |

### 添加新的缺陷类别

**步骤 1**：在 YOLO 训练数据集中添加新类别标注。

**步骤 2**：重新训练模型，更新 `.pt` 文件。

**步骤 3**：在 `MainWindow.cs` 中更新缺陷类别解析逻辑：

```csharp
// 在结果解析代码中添加新类别映射
// 例如添加类别 6: "裂纹"
case 6:
    defectType = "裂纹";
    break;
```

**步骤 4**：更新 `ResultData.cs` 中的数据模型（如需要）。

---

## 8. 性能调优

### 8.1 推理速度优化

| 优化策略 | 方法 | 预期效果 |
|---------|------|---------|
| 使用 GPU 推理 | 设置 `device="cuda:0"` | 速度提升 5-20 倍 |
| FP16 半精度 | 设置 `half=True` | 速度提升 30-50%，精度略有下降 |
| 减小输入尺寸 | 设置 `imgsz=320` | 速度提升 2-4 倍，精度有所下降 |
| 使用轻量模型 | 选用 YOLOv5 | 速度最快，精度相对较低 |
| 批量推理 | 一次处理多张图片 | 提升 GPU 利用率 |
| 模型导出 | 导出为 ONNX/TensorRT 格式 | 显著提升推理速度 |

### 8.2 精度优化

| 优化策略 | 方法 | 预期效果 |
|---------|------|---------|
| 使用高精度模型 | 选用 YOLOv8-EBS | 精度最高 |
| 调整置信度阈值 | 降低 `conf` 值 | 召回率提高，但可能增加误检 |
| 调整 IoU 阈值 | 调整 `iou` 值 | 控制重复检测的过滤强度 |
| 增大输入尺寸 | 设置 `imgsz=1280` | 小缺陷检测精度提升 |
| 数据增强训练 | 在训练集中增加数据变换 | 模型泛化能力提升 |
| 测试时增强 (TTA) | 启用 `augment=True` | 精度提升 1-3%，速度降低 |

### 8.3 内存管理

```python
import torch
import gc

# 推理后释放 GPU 显存
torch.cuda.empty_cache()
gc.collect()

# 限制 GPU 显存使用
torch.cuda.set_per_process_memory_fraction(0.8, device=0)
```

---

## 9. 故障排查

### 常见问题及解决方案

#### 问题 1：检测结果不显示

**可能原因**：
- `detect/` 或 `text/` 目录不存在
- Python 检测脚本执行出错
- 文件权限不足

**解决方案**：
```bash
# 检查目录是否存在
ls -la resource/ detect/ text/

# 手动运行检测脚本检查错误
python scripts/detect.py --source resource/test.jpg --verbose
```

#### 问题 2：模型加载失败

**可能原因**：
- `.pt` 文件损坏或不完整
- PyTorch 版本不兼容
- CUDA 版本不匹配

**解决方案**：
```bash
# 验证模型文件完整性
python -c "import torch; model = torch.load('models/YOLOv8-EBS.pt'); print('Model loaded successfully')"

# 检查 PyTorch 和 CUDA 版本
python -c "import torch; print(f'PyTorch: {torch.__version__}'); print(f'CUDA available: {torch.cuda.is_available()}')"
```

#### 问题 3：GPU 推理不可用

**可能原因**：
- 未安装 CUDA 版本的 PyTorch
- GPU 驱动版本过低
- 显存不足

**解决方案**：
```bash
# 检查 GPU 状态
nvidia-smi

# 安装 CUDA 版本的 PyTorch
pip install torch torchvision --index-url https://download.pytorch.org/whl/cu118
```

#### 问题 4：检测速度过慢

**可能原因**：
- 使用 CPU 进行推理
- 输入图像分辨率过高
- 模型过大

**解决方案**：
- 切换到 GPU 推理
- 减小 `imgsz` 参数值
- 使用更轻量的模型（如 YOLOv5）
- 启用 FP16 半精度推理

#### 问题 5：数据库连接失败

**可能原因**：
- MySQL 服务未启动
- 连接参数错误

**解决方案**：
```bash
# 检查 MySQL 服务状态
systemctl status mysql

# 测试数据库连接
mysql -u root -p -h localhost -P 3306 YoloCheckRoad
```

---

## 10. 扩展指南

### 10.1 集成新的 AI 模型框架

如需集成非 YOLO 的检测模型（如 Faster R-CNN、SSD、DETR 等），需要：

1. **创建适配器脚本**：编写 Python 适配器脚本，使其输出格式与系统期望一致（见 [5.3 检测结果格式](#53-检测结果格式)）。

2. **标准化输出接口**：确保适配器脚本将标注图片写入 `detect/`，元数据写入 `text/`。

3. **注册模型**：在前端 `MainWindow.cs` 的模型下拉框中添加条目。

### 10.2 实现实时视频流检测

当前系统支持视频文件的逐帧检测。若需实现实时视频流检测：

```python
import cv2
from ultralytics import YOLO

model = YOLO("models/YOLOv8-EBS.pt")
cap = cv2.VideoCapture(0)  # 摄像头设备号

while cap.isOpened():
    ret, frame = cap.read()
    if not ret:
        break

    # 执行检测
    results = model.predict(frame, conf=0.25, imgsz=640)

    # 保存结果
    annotated_frame = results[0].plot()
    cv2.imwrite("detect/latest_frame.jpg", annotated_frame)

    # 保存元数据
    with open("text/latest_frame.txt", "w") as f:
        for box in results[0].boxes:
            cls = int(box.cls[0])
            conf = float(box.conf[0])
            x1, y1, x2, y2 = box.xyxy[0].tolist()
            f.write(f"{cls} {x1:.1f} {y1:.1f} {x2:.1f} {y2:.1f} {conf:.2f}\n")

cap.release()
```

### 10.3 配置模型自动更新

可以设置定时任务自动下载和更新模型：

```python
import os
import hashlib
import urllib.request

MODEL_URL = "https://your-model-server.com/models/latest.pt"
MODEL_PATH = "models/YOLOv8-EBS.pt"

def check_and_update_model():
    """检查并更新模型文件"""
    # 下载最新模型的校验和
    response = urllib.request.urlopen(MODEL_URL + ".md5")
    remote_md5 = response.read().decode().strip()

    # 计算本地模型的校验和
    if os.path.exists(MODEL_PATH):
        with open(MODEL_PATH, "rb") as f:
            local_md5 = hashlib.md5(f.read()).hexdigest()

        if local_md5 == remote_md5:
            print("Model is up to date.")
            return

    # 下载新模型
    print("Downloading updated model...")
    urllib.request.urlretrieve(MODEL_URL, MODEL_PATH)
    print("Model updated successfully.")
```

### 10.4 批量检测模式

对于需要批量处理大量图片的场景：

```python
import os
from pathlib import Path
from ultralytics import YOLO

def batch_detect(input_dir, output_dir, text_dir, model_path, **kwargs):
    """批量检测目录中的所有图片"""
    model = YOLO(model_path)
    image_extensions = {".jpg", ".jpeg", ".png", ".bmp"}

    images = [
        f for f in Path(input_dir).iterdir()
        if f.suffix.lower() in image_extensions
    ]

    print(f"Found {len(images)} images to process.")

    for img_path in images:
        results = model.predict(
            source=str(img_path),
            conf=kwargs.get("conf", 0.25),
            iou=kwargs.get("iou", 0.45),
            imgsz=kwargs.get("imgsz", 640),
            device=kwargs.get("device", "auto"),
        )

        # 保存标注图片
        annotated = results[0].plot()
        output_path = os.path.join(output_dir, img_path.name)
        from PIL import Image
        Image.fromarray(annotated[..., ::-1]).save(output_path)

        # 保存元数据
        txt_path = os.path.join(text_dir, img_path.stem + ".txt")
        with open(txt_path, "w") as f:
            for box in results[0].boxes:
                cls = int(box.cls[0])
                conf = float(box.conf[0])
                x1, y1, x2, y2 = box.xyxy[0].tolist()
                f.write(f"{cls} {x1:.1f} {y1:.1f} {x2:.1f} {y2:.1f} {conf:.2f}\n")

    print(f"Batch detection completed. Processed {len(images)} images.")


if __name__ == "__main__":
    batch_detect(
        input_dir="resource/",
        output_dir="detect/",
        text_dir="text/",
        model_path="models/YOLOv8-EBS.pt",
        conf=0.25,
        imgsz=640,
    )
```

---

## 附录

### A. 环境变量配置

| 环境变量 | 说明 | 示例值 |
|---------|------|-------|
| `CHECKPCB_MODEL_DIR` | 模型文件目录 | `./models` |
| `CHECKPCB_INPUT_DIR` | 输入图片目录 | `./resource` |
| `CHECKPCB_OUTPUT_DIR` | 输出图片目录 | `./detect` |
| `CHECKPCB_TEXT_DIR` | 元数据输出目录 | `./text` |
| `CHECKPCB_DEVICE` | 推理设备 | `cuda:0` |
| `CHECKPCB_CONF_THRESHOLD` | 置信度阈值 | `0.25` |

### B. 推荐硬件配置

| 组件 | 最低配置 | 推荐配置 |
|------|---------|---------|
| CPU | Intel i5 / AMD Ryzen 5 | Intel i7 / AMD Ryzen 7 以上 |
| 内存 | 8 GB | 16 GB 以上 |
| GPU | 无（仅 CPU 推理） | NVIDIA RTX 3060 (6GB VRAM) 以上 |
| 存储 | 10 GB 可用空间 | SSD，50 GB 以上可用空间 |
| 操作系统 | Windows 10 | Windows 10/11 64-bit |

### C. 参考资源

- [Ultralytics YOLOv8 官方文档](https://docs.ultralytics.com/)
- [YOLOv7 GitHub 仓库](https://github.com/WongKinYiu/yolov7)
- [YOLOv5 GitHub 仓库](https://github.com/ultralytics/yolov5)
- [PyTorch 官方文档](https://pytorch.org/docs/)
- [OpenCV Python 教程](https://docs.opencv.org/4.x/d6/d00/tutorial_py_root.html)
