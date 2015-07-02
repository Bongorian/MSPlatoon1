unsigned int data[16];
String convert;
int ir = 2;
int bit0 = 3;
int bit1 = 4;
int bit2 = 5;
int bit3 = 6;
int i = 0;

void setup()
{
     Serial.begin(57600);                   // パソコン(ArduinoIDE)とシリアル通信の準備を行う
     pinMode(2,INPUT);      // 赤外線受信モジュールに接続したピンをデジタル入力に設定
     pinMode(bit0,OUTPUT);
     pinMode(bit1,OUTPUT);
     pinMode(bit2,OUTPUT);
     pinMode(bit3,OUTPUT);
     digitalWrite(bit0, LOW);
     digitalWrite(bit1, LOW);
     digitalWrite(bit2, LOW);
     digitalWrite(bit3, LOW);
}

void loop(){
     unsigned long t, pre_t;
     int state, preState;
     preState = digitalRead(2);         // 現在の状態を取得
     pre_t = 0;  
     while(1) {
         state = digitalRead(2);
         if(state != preState) {            // 状態が変化するまで待つ
             preState = state;
             t = micros();                  // 時間(マイクロ秒を)を記録
             if(pre_t != 0) {
              long x = (t-pre_t) / 500;
               switch ( x ){
                 case 1:
                   data [i] = 0;
                   break;
                 
                 case 3:
                   data [i] = 1;
                   break;
                   
                 default:
                   data [i] = 2;
                   break;
               }  
               Serial.println(t-pre_t);
               i++;
          }
             pre_t = t;
         }   
        if( i == 16 ){break;}
     }
        for (int j = 0; j<16;j++){
          convert += data[j];
           }
          int p = convert.indexOf('10101101');
          if (p != -1){
              if (data[p+8] == 1) digitalWrite(bit0, HIGH);
              else digitalWrite(bit0, LOW);
              if (data[p+9] == 1) digitalWrite(bit1, HIGH);
              else digitalWrite(bit1, LOW);
              if (data[p+10] == 1) digitalWrite(bit2, HIGH);
              else digitalWrite(bit2, LOW);
              if (data[p+11] == 1) digitalWrite(bit3, HIGH);
              else digitalWrite(bit3, LOW);
              Serial.println("*");
               Serial.println(data[p+8]);
               Serial.println(data[p+9]);
               Serial.println(data[p+10]);
               Serial.println(data[p+11]);
              delay(1000);
            }
              digitalWrite(bit0, LOW);
              digitalWrite(bit1, LOW);
              digitalWrite(bit2, LOW);
              digitalWrite(bit3, LOW);
              i = 0; 
              convert ="";
   }
