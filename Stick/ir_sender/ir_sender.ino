// LEDをつなぐピンを定義
int led_pins[] = { 5, 6, 7};
int ir_out = 4; //
boolean SendMode=true;
int pushBtn=0;
int btn_pin=10;
int color_number=0;
//データ
  unsigned int check[] = {160,60,160,60,160,160,60,160};
  unsigned int value[] = {0,0,0,0,0,0,0,0,0,0,0,0,0};
  unsigned int result[] = {0,0,0,0,300}; // 尻に余計なＢｉｔを付ける
  unsigned long us = micros();
 
// 初期化
void setup(){
  Serial.begin(57600);
  pinMode(ir_out, OUTPUT);
  int i;// LEDのピンを出力に
  for (i=0; i<3; i++) {
  pinMode(led_pins[i], OUTPUT);
  }

 pinMode(btn_pin,INPUT);
}

void createResult(unsigned int col1,unsigned int col2,unsigned int col3,unsigned int col4){
  unsigned int res[] = {};
  result[0] = col1;
  result[1] = col2;
  result[2] = col3;
  result[3] = col4;
}

void addResult(){
  Serial.println("Start addResult");
  for(int i = 0; i < 13; i++){
    if(i < 8){
      value[i] = check[i];
    } else {
      value[i] = result[i - 8];
       Serial.println(result[i - 8]);
    }
  }
}

 
void sendSignal() {
  int dataSize = sizeof(value) / sizeof(value[0]);
  for (int cnt = 0; cnt < dataSize; cnt++) {
    unsigned long len = value[cnt]*10;  // dataは10us単位でON/OFF時間を記録している
    unsigned long us = micros();
    do {
      digitalWrite(ir_out, 1 - (cnt&1)); // cntが偶数なら赤外線ON、奇数ならOFFのまま
          //  delayMicroseconds();
      digitalWrite(ir_out, 0);
    //  delayMicroseconds();
    } while (long(us + len - micros()) > 0); // 送信時間に達するまでループ
  }
}

void ButtonCheck(){
  pushBtn=0;
  while(digitalRead(btn_pin)==LOW){
    Serial.println("Btton wait...");
    Serial.print("MY COLOR IS ");
    Serial.println(checkColor(color_number));
  }
  while(digitalRead(btn_pin)==HIGH){
    Serial.println("Pushing Button");
    pushBtn++;
    delay(1000);
  }
  if(pushBtn>6) SendMode=false;
}

void ColorSet(){
  pushBtn=0;
  while(digitalRead(btn_pin)==LOW);

  while(digitalRead(btn_pin)==HIGH){
    pushBtn++;
    delay(1000);
  }
  if(pushBtn>6){ 
    SendMode=true;
  }else{
    color_number++;}
}

String checkColor(int num){
   switch (num%6){
      case 0:
          //Serial.println("COLOR IS RED");
          digitalWrite(led_pins[0],HIGH);
          digitalWrite(led_pins[1],LOW);
          digitalWrite(led_pins[2],LOW);
          return "RED";
          
      case 1:
          //Serial.println("COLOR IS GREEN");
          digitalWrite(led_pins[0],LOW);
          digitalWrite(led_pins[1],HIGH);
          digitalWrite(led_pins[2],LOW);
          return "GREEN";
      case 2:
          //Serial.println("COLOR IS BRUE");
          digitalWrite(led_pins[0],LOW);
          digitalWrite(led_pins[1],LOW);
          digitalWrite(led_pins[2],HIGH);
          return "BRUE";
      case 3:
          //Serial.println("COLOR IS BLACK");
          digitalWrite(led_pins[0],HIGH);
          digitalWrite(led_pins[1],HIGH);
          digitalWrite(led_pins[2],LOW);
          return "ORANGE";
       case 4:
          //Serial.println("COLOR IS BLACK");
          digitalWrite(led_pins[0],HIGH);
          digitalWrite(led_pins[1],LOW);
          digitalWrite(led_pins[2],HIGH);
          return "PURPLE";
       case 5:
          //Serial.println("COLOR IS BLACK");
           digitalWrite(led_pins[0],LOW);
          digitalWrite(led_pins[1],HIGH);
          digitalWrite(led_pins[2],HIGH);
          return "YELLOW";
     default:
          return "";
    }
}
// メインループ
void loop() {
  
  if(SendMode==true){
    ButtonCheck();
    Serial.println("Send Mode...");
 if(color_number%6==0){   //色によって杖番号の選択
 createResult(160,160,60,60); // 0と1で160と60にｒｙ
 }else if(color_number%6==1){
  createResult(60,60,160,160);
 }else if(color_number%6==2){
  createResult(160,160,60,60);
 }
 addResult();
 sendSignal();
 //delay(2000);
  }else{
    Serial.println("Setting Mode...");
    Serial.print("COLOR IS  ");
    Serial.println(checkColor(color_number));
    ColorSet();

  }
}
