
import cv2

 
# 创建摄像机对象
cap = cv2.VideoCapture(0)
    
# 检查摄像机是否成功打开
if not cap.isOpened():
    print("无法打开摄像机")
    exit()
    
while True:
    # 读取摄像机的每一帧
    ret, frame = cap.read()
    
    # 显示摄像机的实时画面
    cv2.imshow("摄像机", frame)
    
    # 按下 'q' 键退出循环
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break
    
# 释放摄像机对象和关闭窗口
cap.release()
cv2.destroyAllWindows()



if __name__ == '__main__':
    print("显示摄像机")
