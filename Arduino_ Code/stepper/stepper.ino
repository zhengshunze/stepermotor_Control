// 0416
//引入程式庫
#include <AccelStepper.h>
//定義腳位及實體化
AccelStepper stepper1(AccelStepper::FULL2WIRE, 2, 3);
AccelStepper stepper2(AccelStepper::FULL2WIRE, 4, 5);
AccelStepper stepper3(AccelStepper::FULL2WIRE, 6, 7);
//宣告變數
char opt; //馬達的控制命令字元
long int data[3];                   //馬達的控制項
long int stepperNum[3] = {1, 1, 1}; //馬達的控制編
long StartTime = 0;                 //初始時間
//邏輯判斷
bool runallowed = false;
bool newData = false;
bool lastStepPosition = false;

void setup()
{

  Serial.begin(500000); //鮑率設定為500000
  Serial.setTimeout(10);
  StartTime = millis();
  //Serial.println(F(" "));
  //Serial.println(F("Please type M to show Motor Commands Menu! "));
  // Serial.println(F(" "));

  stepper1.setMaxSpeed(8000.0);     //第一層馬達最大速度9000
  stepper1.setAcceleration(2000.0); //第一層馬達加速度9000
  //stepper1.moveTo(-640000);

  stepper2.setMaxSpeed(400.0);      //第二層馬達最大速度400
  stepper2.setAcceleration(3000.0); //第二層馬達加速度3000
  // stepper2.moveTo(-640000);
  stepper3.setMaxSpeed(200.0);     //第三層馬達最大速度200
  stepper3.setAcceleration(300.0); //第三層馬達加速度400
  //stepper3.moveTo(+64000);

  stepper1.disableOutputs();
  stepper2.disableOutputs();
  stepper3.disableOutputs();

}

void loop()
{
  check();
  RunAllMotor();
  RunTheMotor();
  stepper1.run();
  stepper2.run();
  stepper3.run();
}
//定義單個馬達做動情況
void RunTheMotor()
{
  switch (opt)
  {
    case 'X':

      if (stepper1.distanceToGo() != 0)
      {
        stepper1.enableOutputs();
        stepper1.run();
        lastStepPosition = true;
        if ((millis() - StartTime) >= 500)
        {
          StartTime = millis();
          Serial.print("C");
          Serial.println(stepper1.currentPosition());
        }
        if (!(stepper1.distanceToGo() != 0))
        {
          stepper1.disableOutputs();
          if (lastStepPosition == true)
          {
            Serial.print("C");
            Serial.println(stepper1.currentPosition());
            lastStepPosition = false;
          }
        }
      }
      break;

    case 'Y':

      if (stepper2.distanceToGo() != 0)
      {
        stepper2.enableOutputs();
        stepper2.run();
        lastStepPosition = true;
        if ((millis() - StartTime) >= 500)
        {
          StartTime = millis();
          Serial.print("P");
          Serial.println(stepper2.currentPosition());
        }
        if (!(stepper2.distanceToGo() != 0))
        {
          stepper2.disableOutputs();
          if (lastStepPosition == true)
          {
            Serial.print("P");
            Serial.println(stepper2.currentPosition());
            lastStepPosition = false;
          }
        }
      }
      break;

    case 'Z':
      if (stepper3.distanceToGo() != 0)
      {
        stepper3.enableOutputs();
        stepper3.run();
        lastStepPosition = true;
        if ((millis() - StartTime) >= 500)
        {
          StartTime = millis();
          Serial.print("T");
          Serial.println(stepper3.currentPosition());
        }
        if (!(stepper3.distanceToGo() != 0))
        {
          stepper3.disableOutputs();
          if (lastStepPosition == true)
          {
            Serial.print("T");
            Serial.println(stepper3.currentPosition());
            lastStepPosition = false;
          }
        }
      }
      break;
    default:
      break;
  }
}
//定義所有馬達做動情況
void RunAllMotor() //function for the motor
{
  switch (opt)
  {
    case 'R':

      if (stepper1.distanceToGo() != 0)
      {
        stepper1.enableOutputs();
        stepper1.run();

        lastStepPosition = true;
        if ((millis() - StartTime) >= 400)
        {
          StartTime = millis();
          Serial.print("\n");
          Serial.print("C");
          Serial.println(stepper1.currentPosition());
          Serial.print("P");
          Serial.println(stepper2.currentPosition());
          Serial.print("T");
          Serial.println(stepper3.currentPosition());

        }
        if (!(stepper1.distanceToGo() != 0))
        {
          stepper1.disableOutputs();
          if (lastStepPosition == true)
          {
            Serial.print("C");
            Serial.println(stepper1.currentPosition());
            lastStepPosition = false;
          }
        }
        stepper2.run();
        stepper3.run();
      }
      else if (stepper2.distanceToGo() != 0)
      {
        stepper2.enableOutputs();

        stepper2.run();

        lastStepPosition = true;
        if ((millis() - StartTime) >= 400)
        {
          StartTime = millis();
          Serial.print("\n");
          Serial.print("C");
          Serial.println(stepper1.currentPosition());
          Serial.print("P");
          Serial.println(stepper2.currentPosition());
          Serial.print("T");
          Serial.println(stepper3.currentPosition());
        }
        if (!(stepper2.distanceToGo() != 0))
        {
          stepper2.disableOutputs();
          if (lastStepPosition == true)
          {
            Serial.print("P");
            Serial.println(stepper2.currentPosition());
            lastStepPosition = false;
          }
        }
        stepper1.run();
        stepper3.run();
      }
      else if (stepper3.distanceToGo() != 0)
      {
        stepper3.enableOutputs();
        stepper3.run();
        lastStepPosition = true;
        if ((millis() - StartTime) >= 400)
        {
          StartTime = millis();
          Serial.print("\n");
          Serial.print("C");
          Serial.println(stepper1.currentPosition());
          Serial.print("P");
          Serial.println(stepper2.currentPosition());
          Serial.print("T");
          Serial.println(stepper3.currentPosition());

        }
        if (!(stepper3.distanceToGo() != 0))
        {
          stepper3.disableOutputs();
          if (lastStepPosition == true)
          {
            Serial.print("T");
            Serial.println(stepper3.currentPosition());
            lastStepPosition = false;
          }
        }
      }
      stepper1.run();
      stepper2.run();
      break;
    default:
      break;
  }
}

void check()
{
  if (Serial.available() > 0)// 檢查端口是否有數據等待傳輸
  {
    opt = Serial.read(); // 讀取馬達指令中的信息
    // Serial.print(F("opt="));         // 在序列視窗中輸出 " cmd = "
    // Serial.println(opt);             // 輸出cmd後換行
    data[0] = Serial.parseInt(); // 讀取馬達1指令中整數参數

    data[1] = Serial.parseInt(); // 讀取馬達2指令中整數参數

    data[2] = Serial.parseInt(); // 讀取馬達3指令中整數参數
    newData = true;
    usercmd(); // 呼叫副程式runUsrCmd()
  }
}

void usercmd()
{
  if (newData == true) //we only enter this long switch-case statement if there is a new command from the computer
  {

    switch (opt)
    {
      //currentPosition
      case 'C': //利用currentPosition獲得目前馬達輸出的位置
        Serial.print("C");
        Serial.println(stepper1.currentPosition());
        break;
      case 'P': //利用currentPosition獲得目前馬達輸出的位置
        Serial.print("P");
        Serial.println(stepper2.currentPosition());
        break;
      case 'T': //利用currentPosition獲得目前馬達輸出的位置
        Serial.print("T");
        Serial.println(stepper3.currentPosition());
        break;

      //個別moveTo
      case 'v': //利用moveTo使馬達運作到指定絕對位置值
        if (stepperNum[0] == 1)
        {
          //Serial.print(F("set stepper1 moveTo: "));
          //Serial.println(data[0]);
          stepper1.moveTo(data[0]);
          runallowed = true;
        }
        break;
      case 'e':
        if (stepperNum[1] == 1)
        {
          //Serial.print(F("set stepper2 moveTo: "));
          //Serial.println(data[1]);
          stepper2.moveTo(data[1]);
          runallowed = true;
        }
        break;
      case 't':
        if (stepperNum[2] == 1)
        {
          //Serial.print(F("set stepper3 moveTo: "));
          //Serial.println(data[2]);
          stepper3.moveTo(data[2]);
          runallowed = true;
        }
        break;
      //整體moveTo
      case 'h': //利用moveTo使馬達運作到指定絕對位置值
        if (stepperNum[0] == 1) {
          //Serial.print(F("Motor1 'moveTo': "));
          //Serial.println(data[0]);
          stepper1.moveTo(data[0]);
        }
        if (stepperNum[1] == 1) {
          //Serial.print(F("Motor2 'moveTo': "));
          //Serial.println(data[1]);
          stepper2.moveTo(data[1]);
        }
        if (stepperNum[2] == 1) {
          //Serial.print(F("Motor3 'moveTo': "));
          //Serial.println(data[2]);
          stepper3.moveTo(data[2]);
        }
        break;

      //個別move
      case 'm': //利用move使馬達運作到指定相對位置值

        if (stepperNum[0] == 1)
        {
          //Serial.print(F("Motor1 'move': "));
          //Serial.println(data[0]);
          stepper1.move(data[0]);
        }

        if (stepperNum[1] == 1)
        {
          //Serial.print(F("Motor2 'move': "));
          //Serial.println(data[1]);
          stepper2.move(data[1]);
        }

        if (stepperNum[2] == 1)
        {
          //Serial.print(F("Motor3 'move': "));
          //Serial.println(data[2]);
          stepper3.move(data[2]);
        }
        break;
      //setCurrentPosition
      case 's': //利用setCurrentPosition設定目前位置為指定位置值
        if (stepperNum[0] == 1)
        {
          //Serial.print(F("Motor1 'setCurrentPosition': "));
          //Serial.println(data[0]);
          stepper1.setCurrentPosition(data[0]);
        }
        if (stepperNum[1] == 1)
        {
          //Serial.print(F("Motor2 'setCurrentPosition': "));
          //Serial.println(data[1]);
          stepper2.setCurrentPosition(data[1]);
        }
        if (stepperNum[2] == 1)
        {
          //Serial.print(F("Motor3 'setCurrentPosition': "));
          //Serial.println(data[2]);
          stepper3.setCurrentPosition(data[2]);
        }
        break;

      //個別setAcceleration
      case 'a': //利用setAcceleration設定加速度值
        if (stepperNum[0] == 1)
        {
          //Serial.print(F("set stepper1 Acceleration: "));
          //Serial.println(data[0]);
          stepper1.setAcceleration(data[0]);
        }
        break;
      case 'b':
        if (stepperNum[1] == 1)
        {
          //Serial.print(F("set stepper2 Acceleration: "));
          //Serial.println(data[1]);
          stepper2.setAcceleration(data[1]);
        }
        break;
      case 'c':
        if (stepperNum[2] == 1)
        {
          //Serial.print(F("set stepper3 Acceleration: "));
          //Serial.println(data[2]);
          stepper3.setAcceleration(data[2]);
        }
        break;

      //整體setAcceleration
      case 'q':  //利用setAcceleration設定加速度值
        if (stepperNum[0] == 1) {
          //Serial.print(F("Motor1 'setAcceleration': "));
          //Serial.println(data[0]);
          stepper1.setAcceleration(data[0]);
        }
        if (stepperNum[1] == 1) {
          //Serial.print(F("Motor2 'setAcceleration': "));
          //Serial.println(data[1]);
          stepper2.setAcceleration(data[1]);
        }
        if (stepperNum[2] == 1) {
          //Serial.print(F("Motor3 'setAcceleration': "));
          //Serial.println(data[2]);
          stepper3.setAcceleration(data[2]);
        }
        break;

      //個別setMaxSpeed
      case 'x': //利用setMaxSpeed函数設定最大速度值
        if (stepperNum[0] == 1)
        {
          //Serial.print(F("set stepper1 MaxSpeed: "));
          //Serial.println(data[0]);
          stepper1.setMaxSpeed(data[0]);
        }
        break;
      case 'y': //利用setMaxSpeed函数設定最大速度值

        if (stepperNum[1] == 1)
        {
          //Serial.print(F("set stepper2 MaxSpeed: "));
          //Serial.println(data[1]);
          stepper2.setMaxSpeed(data[1]);
        }
        break;
      case 'z': //利用setMaxSpeed函数設定最大速度值

        if (stepperNum[2] == 1)
        {
          //Serial.print(F("set stepper3 MaxSpeed: "));
          //Serial.println(data[2]);
          stepper2.setMaxSpeed(data[2]);
        }

      //整體setMaxSpeed
      case 'n':  //利用setMaxSpeed函数設定最大速度值
        if (stepperNum[0] == 1) {
          //Serial.print(F("Motor1 'setMaxSpeed' "));
          //Serial.println(data[0]);
          stepper1.setMaxSpeed(data[0]);
        }
        if (stepperNum[1] == 1) {
          //Serial.print(F("Motor2 'setMaxSpeed' "));
          //Serial.println(data[1]);
          stepper2.setMaxSpeed(data[1]);
        }
        if (stepperNum[2] == 1) {
          //Serial.print(F("Motor3 'setMaxSpeed' "));
          //Serial.println(data[2]);
          stepper2.setMaxSpeed(data[2]);
        }
        break;

      //顯示選單
      case 'M':
        showmeun();
        break;

      //顯示目前馬達設定參數值
      case 'V':
        showvalue();
        break;

      //重置所有馬達
      case 'S':
        runallowed = false;
        stepper1.disableOutputs();
        stepper1.setCurrentPosition(0);
        stepper2.disableOutputs();
        stepper2.setCurrentPosition(0);
        stepper3.disableOutputs();
        stepper3.setCurrentPosition(0);
        break;
      //重置所有馬達
      case 'E':
        stepper1.run();
        stepper2.run();
        stepper3.run();
        stepper1.enableOutputs();
        stepper2.enableOutputs();
        stepper3.enableOutputs();
        break;
      //個別重置所有馬達
      case 'U':
        runallowed = false;
        stepper1.disableOutputs();
        stepper1.setCurrentPosition(0);
        break;
      case 'D':
        runallowed = false;
        stepper2.disableOutputs();
        stepper2.setCurrentPosition(0);
        break;
      case 'B':
        runallowed = false;
        stepper3.disableOutputs();
        stepper3.setCurrentPosition(0);
        break;

      default: // 未知指令
        break;
    }
  }
  newData = false;
}




//馬達指令列表
void showmeun()
{
  Serial.println(F("Motor Control Commands : "));
  Serial.println(F(" S : Stop All motors "));
  Serial.println(F(" E : Enable All motors "));
  Serial.println(F(" o : currentPosition "));
  Serial.println(F(" v : moveTo"));
  Serial.println(F(" m : move"));
  Serial.println(F(" s : setCurrentPosition"));
  Serial.println(F(" a : setAcceleration"));
  Serial.println(F(" x : setMaxSpeed"));
  Serial.println(F(" d : set witch motor working"));
}


//馬達設定參數值
void showvalue()
{
  Serial.println(F(" "));
  Serial.print(F("目前馬達1設定值: "));
  Serial.println(data[0]); // 輸出馬達1的參數值

  Serial.print(F("目前馬達2設定值: "));
  Serial.println(data[1]); // 輸出馬達2的參數值

  Serial.print(F("目前馬達3設定值: "));
  Serial.print(data[2]); // 輸出馬達3的參數值
}
