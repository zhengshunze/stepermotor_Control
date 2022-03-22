// 0410

#include <AccelStepper.h>
AccelStepper stepper1(AccelStepper::FULL2WIRE, 2, 3);
AccelStepper stepper2(AccelStepper::FULL2WIRE, 4, 5);
AccelStepper stepper3(AccelStepper::FULL2WIRE, 6, 7);

char opt; //馬達的控制命令字元

long int data[3];                   //馬達的控制項
long int stepperNum[3] = {1, 1, 1}; //馬達的控制編
long StartTime = 0;
long receivedSteps = 0;
long receivedSpeed = 0;

bool runallowed = false;
bool newData = false;
bool lastStepPosition = false;

void setup()
{
  Serial.begin(500000); //鮑率設定為500000
  Serial.setTimeout(10);
  Serial.println(F(" "));
  Serial.println(F("Please type M to show Motor Commands Menu! "));
  // Serial.println(F(" "));

  stepper1.setMaxSpeed(8000.0);     //第一層馬達最大速度9000
  stepper1.setAcceleration(9000.0); //第一層馬達加速度9000
  //stepper1.moveTo(-640000);

  stepper2.setMaxSpeed(400.0);      //第二層馬達最大速度400
  stepper2.setAcceleration(3000.0); //第二層馬達加速度3000
  // stepper2.moveTo(-640000);
  stepper3.setMaxSpeed(200.0);     //第三層馬達最大速度200
  stepper3.setAcceleration(300.0); //第三層馬達加速度400
  //stepper3.moveTo(+64000);

  stepper1.disableOutputs(); //disable outputs
  stepper2.disableOutputs(); //disable outputs
  stepper3.disableOutputs(); //disable outputs
}

void loop()
{

  check();
  RunTheMotor();

}

void RunTheMotor() //function for the motor
{

  if (stepper1.distanceToGo() != 0)
  {
    stepper1.enableOutputs();
    stepper1.run();
    lastStepPosition = true;
    if ((millis() - StartTime) >= 400)
    {
      StartTime = millis();
      Serial.print("L");
      Serial.println(stepper1.currentPosition()); //Print the message
    }

    if (!(stepper1.distanceToGo() != 0))
    {
      stepper1.disableOutputs();
      if (lastStepPosition == true)
      {
        Serial.print("L");
        Serial.println(stepper1.currentPosition());
        lastStepPosition = false;
      }
    }

  }

  else if (stepper2.distanceToGo() != 0)
  {
    stepper2.enableOutputs();
    stepper2.run();
    lastStepPosition = true;
    if ((millis() - StartTime) >= 305)
    {
      StartTime = millis();
      Serial.print("O");
      Serial.println(stepper2.currentPosition()); //Print the message
    }

    if (!(stepper2.distanceToGo() != 0))
    {
      stepper1.disableOutputs();
      if (lastStepPosition == true)
      {
        Serial.print("O");
        Serial.println(stepper2.currentPosition());
        lastStepPosition = false;
      }
    }

  }

  else if (stepper3.distanceToGo() != 0)
  {
    stepper3.enableOutputs();
    stepper3.run();
    lastStepPosition = true;
    if ((millis() - StartTime) >= 310)
    {
      StartTime = millis();
      Serial.print("V");
      Serial.println(stepper3.currentPosition()); //Print the message
    }
    
    if (!(stepper3.distanceToGo() != 0))
    {
      stepper1.disableOutputs();
      if (lastStepPosition == true)
      {
        Serial.print("V");
        Serial.println(stepper3.currentPosition());
        lastStepPosition = false;
      }
    }
    
  }
}
/*
  else
  {
    if (stepper1.distanceToGo() == 0)
    {
      stepper1.disableOutputs();
      if (lastStepPosition == true)
      {
        Serial.print("L1");
        Serial.println(stepper1.currentPosition());
        lastStepPosition = false;
      }
    }


  }

  }
*/




void check()
{
  if (Serial.available() > 0)
  { // 檢查端口是否有數據等待傳輸

    opt = Serial.read(); // 讀取馬達指令中的信息
    // Serial.print(F("opt="));         // 在序列視窗中輸出 " cmd = "
    // Serial.println(opt);             // 輸出cmd後換行
    data[0] = Serial.parseInt(); // 讀取馬達1指令中整數参數

    data[1] = Serial.parseInt(); // 讀取馬達2指令中整數参數

    data[2] = Serial.parseInt(); // 讀取馬達3指令中整數参數
    newData = true;
    /* Serial.print(F("目前馬達1設定值: "));
      Serial.println(data[0]);            // 輸出馬達1的參數值

      Serial.print(F("目前馬達2設定值: "));
      Serial.println(data[1]);            // 輸出馬達2的參數值

      Serial.print(F("目前馬達3設定值: "));
      Serial.print(data[2]);              // 輸出馬達3的參數值

      Serial.println(F(""));
    */
    usercmd(); // 呼叫副程式"runUsrCmd()"
  }

  /* stepper1.run(); //使馬達1做動
    stepper2.run(); //使馬達2做動
    stepper3.run(); //使馬達3做動
  */
}
// 副程式"usercmd()"
void usercmd()
{
  if (newData == true) //we only enter this long switch-case statement if there is a new command from the computer
  {

    switch (opt)
    {
      case 'o': //利用currentPosition獲得目前馬達輸出的位置
        Serial.print(F("stepper1 Position: "));
        Serial.println(stepper1.currentPosition());

        /*
                    Serial.print(F("馬達1的目前的位置: "));            //在序列視窗中輸出 " 馬達1的目前的位置:: "
                    Serial.println(stepper1.currentPosition());     //換行輸出馬達1的位置

                    Serial.print(F("馬達2的目前的位置: "));            //在序列視窗中輸出 " 馬達2的目前的位置: "
                    Serial.println(stepper2.currentPosition());    //換行輸出馬達2的位置

                    Serial.print(F("馬達3的目前的位置: "));           //在序列視窗中輸出 " 馬達3的目前的位置: "
                    Serial.println(stepper3.currentPosition());    //換行輸出馬達3的位置

                    Serial.print(F("目前運作中的馬達: "));   //在序列視窗中輸出 " 目前運作中的馬達: "
        */

        break;

      case 'g':

        Serial.print(F("stepper2 Position: "));
        Serial.println(stepper2.currentPosition());
        break;

      case 'v': //利用moveTo使馬達運作到指定絕對位置值

        if (stepperNum[0] == 1)
        {
          Serial.print(F("set stepper1 moveTo: "));
          Serial.println(data[0]);
          stepper1.moveTo(data[0]);
        }
        break;

      case 'e':
        if (stepperNum[1] == 1)
        {
          Serial.print(F("馬達2 'moveTo': "));
          Serial.println(data[1]);
          stepper2.moveTo(data[1]);
        }
        break;
      case 't':
        if (stepperNum[2] == 1)
        {
          Serial.print(F("馬達3 'moveTo': "));
          Serial.println(data[2]);
          stepper3.moveTo(data[2]);
        }
        break;

      case 'm': //利用move使馬達運作到指定相對位置值

        if (stepperNum[0] == 1)
        {
          Serial.print(F("馬達1 'move': "));
          Serial.println(data[0]);
          stepper1.move(data[0]);
        }
        if (stepperNum[1] == 1)
        {
          Serial.print(F("馬達2 'move': "));
          Serial.println(data[1]);
          stepper2.move(data[1]);
        }
        if (stepperNum[2] == 1)
        {
          Serial.print(F("馬達3 'move': "));
          Serial.println(data[2]);
          stepper3.move(data[2]);
        }
        break;

      case 'r': //利用runToNewPosition讓馬達運行到指定絕對位置值
        if (stepperNum[0] == 1)
        {
          Serial.print(F("馬達1 'runToNewPosition': "));
          Serial.println(data[0]);
          stepper1.runToNewPosition(data[0]);
        }
        if (stepperNum[1] == 1)
        {
          Serial.print(F("馬達2 'runToNewPosition': "));
          Serial.println(data[1]);
          stepper2.runToNewPosition(data[1]);
        }
        if (stepperNum[2] == 1)
        {
          Serial.print(F("馬達3 'runToNewPosition': "));
          Serial.println(data[2]);
          stepper3.runToNewPosition(data[2]);
        }
        break;

      case 's': //利用setCurrentPosition設定目前位置為指定位置值
        if (stepperNum[0] == 1)
        {
          Serial.print(F("馬達1 'setCurrentPosition': "));
          Serial.println(data[0]);
          stepper1.setCurrentPosition(data[0]);
        }
        if (stepperNum[1] == 1)
        {
          Serial.print(F("馬達2 'setCurrentPosition': "));
          Serial.println(data[1]);
          stepper2.setCurrentPosition(data[1]);
        }
        if (stepperNum[2] == 1)
        {
          Serial.print(F("馬達3 'setCurrentPosition': "));
          Serial.println(data[2]);
          stepper3.setCurrentPosition(data[2]);
        }
        break;

      case 'a': //利用setAcceleration設定加速度值
        if (stepperNum[0] == 1)
        {
          Serial.print(F("set stepper1 Acceleration: "));
          Serial.println(data[0]);
          stepper1.setAcceleration(data[0]);
        }
        break;
      case 'b':
        if (stepperNum[1] == 1)
        {
          Serial.print(F("set stepper2 Acceleration: "));
          Serial.println(data[1]);
          stepper2.setAcceleration(data[1]);
        }
        break;
      case 'c':
        if (stepperNum[2] == 1)
        {
          Serial.print(F("set stepper3 Acceleration: "));
          Serial.println(data[2]);
          stepper3.setAcceleration(data[2]);
        }
        break;

      case 'x': //利用setMaxSpeed函数設定最大速度值
        if (stepperNum[0] == 1)
        {
          Serial.print(F("set stepper1 MaxSpeed: "));
          Serial.println(data[0]);
          stepper1.setMaxSpeed(data[0]);
        }
        break;
      case 'y': //利用setMaxSpeed函数設定最大速度值

        if (stepperNum[1] == 1)
        {
          Serial.print(F("set stepper2 MaxSpeed: "));
          Serial.println(data[1]);
          stepper2.setMaxSpeed(data[1]);
        }

        break;

      case 'z': //利用setMaxSpeed函数設定最大速度值

        if (stepperNum[2] == 1)
        {
          Serial.print(F("set stepper3 MaxSpeed: "));
          Serial.println(data[2]);
          stepper2.setMaxSpeed(data[2]);
        }

        break;

      case 'd': //設定使哪個馬達做動
        if (data[0] + data[1] + data[2] == 0)
        {
          stepperNum[0] = data[0];
          stepperNum[1] = data[1];
          stepperNum[2] = data[2];
          Serial.print(F("Running Three Motors "));
        }

        if (data[0] + data[1] + data[2] == 1)
        {
          stepperNum[0] = data[0];
          stepperNum[1] = data[1];
          stepperNum[2] = data[2];
          Serial.print(F("Running First Motor "));
          Serial.println((data[0] == 1) ? (data[0]) : (data[1]));
        }

        if (data[0] + data[1] + data[2] == 2)
        {
          stepperNum[0] = data[0];
          stepperNum[1] = data[1];
          stepperNum[2] = data[2];
          Serial.print(F("Running Second Motor "));
          Serial.println((data[0] == 2) ? (data[0]) : (data[1]));
        }

        if (data[0] + data[1] + data[2] == 3)
        {
          stepperNum[0] = data[0];
          stepperNum[1] = data[1];
          stepperNum[2] = data[2];
          Serial.print(F("Running Third Motor "));
          Serial.println((data[0] == 3) ? (data[0]) : (data[1]));
        }
        else if (!(data[0] + data[1] + data[2] == 0 || data[0] + data[1] + data[2] == 1 || data[0] + data[1] + data[2] == 2 || data[0] + data[1] + data[2] == 3))

        {
          Serial.print(F("Motor Number Wrong."));
        }
        break;

      case 'M':
        showmeun();
        break;
      case 'V':
        showvalue();
        break;

      case 'S':
        stepper1.stop();
        stepper2.stop();
        stepper3.stop();
        stepper1.disableOutputs();
        stepper2.disableOutputs();
        stepper3.disableOutputs();
        Serial.println("All Motors are Stopped.");
        break;
      case 'E':
        stepper1.run();
        stepper2.run();
        stepper3.run();
        stepper1.enableOutputs();
        stepper2.enableOutputs();
        stepper3.enableOutputs();
        Serial.println("Enable All motors.");
        break;
      case 'G':
        stepper1.stop();
        stepper1.disableOutputs();
        Serial.println("Motor 1 is stopped.");
        break;
      case 'T':
        stepper2.stop();
        stepper2.disableOutputs();
        Serial.println("Motor 2 is stopped.");
        break;
      case 'A':
        stepper3.stop();
        stepper3.disableOutputs();
        Serial.println("Motor 3 is stopped.");
        break;
      case 'U':
        runallowed = false;             //we still keep running disabled
        stepper1.disableOutputs();      //disable power
        stepper1.setCurrentPosition(0); //Reset current position. "new home"
        // Serial.print("已重置馬達1的當前位置，\n馬達現在正處於任何位置\n皆視為0的新位置"); //Print message
        //Serial.println(stepper1.currentPosition()); //Check position after reset.
        break;
         case 'D':
        runallowed = false;             //we still keep running disabled
        stepper2.disableOutputs();      //disable power
        stepper2.setCurrentPosition(0); //Reset current position. "new home"
        // Serial.print("已重置馬達1的當前位置，\n馬達現在正處於任何位置\n皆視為0的新位置"); //Print message
        //Serial.println(stepper1.currentPosition()); //Check position after reset.
        break;
         case 'B':
        runallowed = false;             //we still keep running disabled
        stepper3.disableOutputs();      //disable power
        stepper3.setCurrentPosition(0); //Reset current position. "new home"
        // Serial.print("已重置馬達1的當前位置，\n馬達現在正處於任何位置\n皆視為0的新位置"); //Print message
        //Serial.println(stepper1.currentPosition()); //Check position after reset.
        break;
      case 'L':                                          //L: Location
        runallowed = false;                              //we still keep running disabled
        stepper1.disableOutputs();                       //disable power
        Serial.print("已將馬達1的目前的位置數值設為零"); //Print the message
        // Serial.println(stepper1.currentPosition()); //Printing the current position in steps.
        break;

      default: // 未知指令
        Serial.println(F("無法辨別的指令!"));
    }
  }
  newData = false;
}




//馬達指令列表
void showmeun()
{
  Serial.print(F("Motor Control Commands : "));
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