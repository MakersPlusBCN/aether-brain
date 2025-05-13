
#include <Adafruit_NeoPixel.h>
#define LED_PIN_PLA_A    2
#define LED_PIN_PLA_B    3
#define LED_PIN_I    4
#define LED_PIN_C    5
#define LED_PIN_S    6
#define LED_PIN_G    7
#define PLAYER_A     8
#define PLAYER_B     9

#define LED_COUNT_SIMBOLS 102
#define LED_COUNT_GESTO 40
#define LED_STATE_GESTO 304
#define LED_INTERIOR 39
#define LED_PULSERA_A 10
#define LED_PULSERA_B 10

boolean estado_on = true;
int period = 1000;
unsigned long time_now = 0;
int side_a_1 = 0;
int side_a_2 = 0;
int side_b_1 = 0;
int side_b_2 = 0;
int s_on = 0;
int s_off = 0;
boolean gesto_on = true;
int segmentos = 0;
int nivel = 0;
uint32_t e_color = (0, 0, 0);
char elemento = "";
int r = 0;
int g = 0;
int b = 0;
int s = 0;
int i_b = 240;
int state = 0;
int valor = -4;
int cuenta = 0;
boolean int_state = true;

int P_A = 0;
int P_B = 0;
int P_Aprev = 0;
int P_Bprev = 0;
unsigned long lastDebounceTimeA = 0;  // the last time the output pin was toggled
unsigned long debounceDelayA = 20;    // the debounce time; increase if the output flickers

unsigned long lastDebounceTimeB = 0;  // the last time the output pin was toggled
unsigned long debounceDelayB = 20;    // the debounce time; increase if the output flickers

bool leds_pulsera_a_on = true;
bool leds_pulsera_b_on = true;


Adafruit_NeoPixel strip_s(LED_COUNT_SIMBOLS, LED_PIN_S, NEO_GRB + NEO_KHZ800);
Adafruit_NeoPixel strip_g(LED_COUNT_GESTO, LED_PIN_G, NEO_GRB + NEO_KHZ800);
Adafruit_NeoPixel strip_c(LED_STATE_GESTO, LED_PIN_C, NEO_GRB + NEO_KHZ800);
//Adafruit_NeoPixel strip_i(LED_INTERIOR, LED_PIN_I, NEO_GRB + NEO_KHZ800);
Adafruit_NeoPixel strip_a(LED_PULSERA_A, LED_PIN_PLA_A, NEO_GRB + NEO_KHZ800);
Adafruit_NeoPixel strip_b(LED_PULSERA_B, LED_PIN_PLA_B, NEO_GRB + NEO_KHZ800);

int incomingByte;

void setup() {
  Serial.begin(9600);

  //Iniciar pines pulsadores (pulseras)
  pinMode(PLAYER_A, INPUT);
  pinMode(PLAYER_B, INPUT);

  //Inicializar leds
  strip_s.begin();
  //strip_s.show();
  //strip_s.setBrightness(100);
  strip_g.begin();
  //strip_g.show();
  //strip_g.setBrightness(100);
  strip_c.begin();
  //strip_c.show();
  //strip_c.setBrightness(100);

  //Reset strip_i
  //strip_i.begin();
  //strip_i.clear();
  //strip_i.show();
  //strip_i.setBrightness(100);
 

  strip_a.begin();
  //strip_b.show();
  //strip_b.setBrightness(100);
  strip_b.begin();
  //strip_b.show();
  //strip_b.setBrightness(100);
  strip_s.clear();
  strip_g.clear();
  strip_c.clear();
  strip_a.clear();
  strip_b.clear();
}

void loop() {

  cuenta++;
    if (cuenta == 5) {
    i_b -= valor;
    if (i_b < 20 || i_b > 240) {
      valor = valor * -1;
    }
    if (int_state == true) {
      state = i_b;
    } else {
      state = 0;
    }

    
   // Serial.println(state);
   
    cuenta = 0;
    }


  

  // read the state of the switch into a local variable:
  int readingA = digitalRead(PLAYER_A);
  int readingB = digitalRead(PLAYER_B);

  // check to see if you just pressed the button
  // (i.e. the input went from LOW to HIGH), and you've waited long enough
  // since the last press to ignore any noise:

  //---- PULSERA A ----
  //LEDS Pulsera A
  if (leds_pulsera_a_on) {
    segment_a(strip_a.Color(state, state, state), 0, LED_PULSERA_A, 0);
  } else {
    strip_a.clear();
    strip_a.show();
  }

  // If the switch changed, due to noise or pressing:
  if (readingA != P_Aprev) {
    // reset the debouncing timer
    lastDebounceTimeA = millis();
  }

  if ((millis() - lastDebounceTimeA) > debounceDelayA) {
    // whatever the reading is at, it's been there for longer than the debounce
    // delay, so take it as the actual current state:

    // if the button state has changed:
    if (readingA != P_A) {
      P_A = readingA;

      if (P_A == HIGH) {
        Serial.write("pulseraA/1\n");
        //strip_a.clear();
        leds_pulsera_a_on = false;
      } else {
        Serial.write("pulseraA/0\n");
        leds_pulsera_a_on = true;
      }
    }
  }

  // save the reading. Next time through the loop, it'll be the lastButtonState:
  P_Aprev = readingA;
  //------------------

  //---- PULSERA B ----
  if (leds_pulsera_b_on) {
    segment_b(strip_b.Color(state, state, state), 0, LED_PULSERA_A, 0);
  } else {
    strip_b.clear();
    strip_b.show();
  }

  // If the switch changed, due to noise or pressing:
  if (readingB != P_Bprev) {
    // reset the debouncing timer
    lastDebounceTimeB = millis();
  }

  if ((millis() - lastDebounceTimeB) > debounceDelayB) {
    // whatever the reading is at, it's been there for longer than the debounce
    // delay, so take it as the actual current state:

    // if the button state has changed:
    if (readingB != P_B) {
      P_B = readingB;

      if (P_B == HIGH) {
        Serial.write("pulseraB/1\n");
        leds_pulsera_b_on = false;
        //strip_b.clear();
      } else {
        Serial.write("pulseraB/0\n");
        leds_pulsera_b_on = true;
      }
    }
  }

  // save the reading. Next time through the loop, it'll be the lastButtonState:
  P_Bprev = readingB;
  //------------------




  if (gesto_on == true) {
    if (millis() >= time_now + period) {
      time_now += period;
      if (estado_on) {
        gesto1_off();
        estado_on = false;
      } else {
        gesto1_on();
        estado_on = true;
      }
    }
  }

  //Listening serial messages
  if (Serial.available() > 0) {
    incomingByte = Serial.read();

    //LEDs signo Water
    if (incomingByte == 'W') {
      r = 0;
      g = 255;
      b = 64;
      water();
    }

    //LEDs signo Fire
    if (incomingByte == 'F') {
      r = 255;
      g = 0;
      b = 0;
      fire();
    }

    //LEDs signo Earth
    if (incomingByte == 'E') {
      r = 255;
      g = 64;
      b = 0;
      earth();
    }

    //LEDs signo Air
    if (incomingByte == 'A') {
      r = 0;
      g = 255;
      b = 255;
      air();
    }
    //Apagar barras de progreso
    if (incomingByte == 'O') {
      strip_s.clear();
      strip_s.show();
    }

    if (incomingByte == 'Q') {//APAGA INDICADOR DE GESTO (H - V - D)
      gesto_on = false;
      strip_g.clear();
      strip_g.show();
    }

    if (incomingByte == '0') {
      r = 0;
      g = 0;
      b = 0;
      nivel = 0;
      segment_c(strip_s.Color(r, g, b), 0, LED_STATE_GESTO, 0);
      strip_c.show();
    }
    //CANTIDAD DE SEGMENTOS
    if (incomingByte == '2') {
      segmentos = 2;
      s = ((LED_STATE_GESTO - 8) / 4 / 2) / 2;

    }
    if (incomingByte == '4') {
      segmentos = 4;
      s = ((LED_STATE_GESTO - 8) / 4 / 2) / 4;
    }
    if (incomingByte == '6') {
      segmentos = 6;
      s = ((LED_STATE_GESTO - 8) / 4 / 2) / 6;
    }
    if (incomingByte == '8') {
      segmentos = 8;
      s = ((LED_STATE_GESTO - 8) / 4 / 2) / 8;
    }

    if (incomingByte == '+') {//ENCIENDE UN NUEVO SEGMENTO
      nivel++;
      if (nivel > segmentos) {
        nivel = segmentos;
      }
      Serial.print(segmentos);
      Serial.print(" - ");
      Serial.println(nivel);
    }
    if (incomingByte == '-') {//APAGA UN SEGMENTO
      nivel--;
      if (nivel < 0) {
        nivel = 0;
      }
      Serial.print(segmentos);
      Serial.print(" - ");
      Serial.println(nivel);
    }

    if (incomingByte == 'H') {
      gesto_on = true;
      side_a_1 = 5;
      side_a_2 = 10;
      side_b_1 = 25;
      side_b_2 = 30;
      fijo();
      s_on = 255;
      s_off = 0;
    }

    if (incomingByte == 'V') {
      gesto_on = true;
      side_a_1 = 0;
      side_a_2 = 5;
      side_b_1 = 20;
      side_b_2 = 25;
      fijo();
      s_on = 255;
      s_off = 0;
    }

    if (incomingByte == 'D') {
      gesto_on = true;
      side_a_1 = 0;
      side_a_2 = 10;
      side_b_1 = 20;
      side_b_2 = 30;
      fijo();
      s_on = 255;
      s_off = 0;
    }

    if (incomingByte == 'I') {
      cambia_sta_int();
    }

if (incomingByte == 'X') {
     
     theaterChase_c(strip_c.Color(200, 200, 200), 80); 
      
    }




    if (segmentos == 2) {
      if (nivel == 0) {
        strip_c.clear();
        strip_c.show();

      }
      if (nivel > 0) {
        strip_c.clear();
        segment_c(strip_c.Color(r, g, b), 4,  ((s * nivel) +  3), 0);
        segment_c(strip_c.Color(r, g, b), 42, ((s * nivel) + 42) - 2, 0);
        segment_c(strip_c.Color(r, g, b), 80, ((s * nivel) + 80) - 1, 0);
        segment_c(strip_c.Color(r, g, b), 118, ((s * nivel) + 118) - 2, 0);
        segment_c(strip_c.Color(r, g, b), 156, ((s * nivel) + 156) - 2, 0);
        segment_c(strip_c.Color(r, g, b), 194, ((s * nivel) + 194) - 2, 0);
        segment_c(strip_c.Color(r, g, b), 232, ((s * nivel) + 232) - 2, 0);
        segment_c(strip_c.Color(r, g, b), 270, ((s * nivel) + 270) - 2, 0);
        strip_c.show();
      }
    }

    if (segmentos == 4) {
      if (nivel > 4) {
        nivel = 4;
      }
      if (nivel == 0) {
        strip_c.clear();
        strip_c.show();

      }
      if (nivel > 0) {
        strip_c.clear();
        segment_c(strip_c.Color(r, g, b), 4,  ((s * nivel) +  3), 0);
        segment_c(strip_c.Color(r, g, b), 42, ((s * nivel) + 42) - 2, 0);
        segment_c(strip_c.Color(r, g, b), 80, ((s * nivel) + 80) - 1, 0);
        segment_c(strip_c.Color(r, g, b), 118, ((s * nivel) + 118) - 2, 0);
        segment_c(strip_c.Color(r, g, b), 156, ((s * nivel) + 156) - 2, 0);
        segment_c(strip_c.Color(r, g, b), 194, ((s * nivel) + 194) - 2, 0);
        segment_c(strip_c.Color(r, g, b), 232, ((s * nivel) + 232) - 2, 0);
        segment_c(strip_c.Color(r, g, b), 270, ((s * nivel) + 270) - 2, 0);
        strip_c.show();

      }

    }

    if (segmentos == 6) {
      if (nivel == 0) {
        strip_c.clear();
        strip_c.show();

      }
      if (nivel > 0) {
        strip_c.clear();
        segment_c(strip_c.Color(r, g, b), 4,  ((s * nivel) +  3), 0);
        segment_c(strip_c.Color(r, g, b), 42, ((s * nivel) + 42) - 2, 0);
        segment_c(strip_c.Color(r, g, b), 80, ((s * nivel) + 80) - 1, 0);
        segment_c(strip_c.Color(r, g, b), 118, ((s * nivel) + 118) - 2, 0);
        segment_c(strip_c.Color(r, g, b), 156, ((s * nivel) + 156) - 2, 0);
        segment_c(strip_c.Color(r, g, b), 194, ((s * nivel) + 194) - 2, 0);
        segment_c(strip_c.Color(r, g, b), 232, ((s * nivel) + 232) - 2, 0);
        segment_c(strip_c.Color(r, g, b), 270, ((s * nivel) + 270) - 2, 0);
        strip_c.show();
      }
    }

    if (segmentos == 8) {
      if (nivel == 0) {
        strip_c.clear();
        strip_c.show();

      }
      if (nivel > 0) {
        strip_c.clear();
        segment_c(strip_c.Color(r, g, b),  4, ((s * nivel) +  6)  , 0);
        segment_c(strip_c.Color(r, g, b), 42, ((s * nivel) + 42) + 2, 0);
        segment_c(strip_c.Color(r, g, b), 80, ((s * nivel) + 80) + 2, 0);
        segment_c(strip_c.Color(r, g, b), 118, ((s * nivel) + 118) + 2, 0);
        segment_c(strip_c.Color(r, g, b), 156, ((s * nivel) + 156) + 2, 0);
        segment_c(strip_c.Color(r, g, b), 194, ((s * nivel) + 194) + 2, 0);
        segment_c(strip_c.Color(r, g, b), 232, ((s * nivel) + 232) + 2, 0);
        segment_c(strip_c.Color(r, g, b), 270, ((s * nivel) + 270) + 3, 0);
        strip_c.show();

      }
    }
  }
}



void segment_s(uint32_t color, int from, int to, int wait) {
  for (int i = from; i < to; i++) { // For each pixel in strip_s...
    strip_s.setPixelColor(i, color);
    strip_s.show();
    delay(wait);
  }
}

void segment_g(uint32_t color, int from, int to, int wait) {
  for (int i = from; i < to; i++) { // For each pixel in strip_s...
    strip_g.setPixelColor(i, color);
    strip_g.show();
    delay(wait);
  }
}

void segment_c(uint32_t color, int from, int to, int wait) {
  for (int i = from; i < to; i++) { // For each pixel in strip_s...
    strip_c.setPixelColor(i, color);
  }
}
/*
void segment_i(uint32_t color, int from, int to, int wait) {
  for (int i = from; i < to; i++) { // For each pixel in strip_s...
    strip_i.setPixelColor(i, color);
    strip_i.show();
    delay(wait);
  }
}
*/

void segment_a(uint32_t color, int from, int to, int wait) {
  for (int i = from; i < to; i++) { // For each pixel in strip_s...
    strip_a.setPixelColor(i, color);
    strip_a.show();
  }
}

void segment_b(uint32_t color, int from, int to, int wait) {
  for (int i = from; i < to; i++) { // For each pixel in strip_s...
    strip_b.setPixelColor(i, color);
    strip_b.show();
  }
}

void water() {
  strip_s.clear();
  segment_s(strip_s.Color(0, 255, 64), 0, 41, 10); //Water
  strip_s.show();
}

void fire() {
  strip_s.clear();
  segment_s(strip_s.Color(255, 0, 0), 13, 27, 10); //Fire
  segment_s(strip_s.Color(255, 0, 0), 41, 49, 10); //Fire
  strip_s.show();
}

void earth() {
  strip_s.clear();
  segment_s(strip_s.Color(255, 64, 0), 15, 25, 10); //Earth
  segment_s(strip_s.Color(255, 64, 0), 49, 58, 10); //Earth
  strip_s.show();
}

void air() {
  strip_s.clear();
  segment_s(strip_s.Color(0, 255, 255), 58, 61, 10); //Air
  segment_s(strip_s.Color(0, 255, 255), 41, 42, 10); //Air
  segment_s(strip_s.Color(0, 255, 255), 27, 30, 10); //Air
  segment_s(strip_s.Color(0, 255, 255), 61, 83, 10); //Air
  segment_s(strip_s.Color(0, 255, 255), 13, 14, 10); //Air
  segment_s(strip_s.Color(0, 255, 255), 83, 84, 10); //Air
  segment_s(strip_s.Color(0, 255, 255), 44, 47, 10); //Air
  segment_s(strip_s.Color(0, 255, 255), 84, 85, 10); //Air
  segment_s(strip_s.Color(0, 255, 255), 25, 26, 10); //Air
  segment_s(strip_s.Color(0, 255, 255), 85, 86, 10); //Air
  segment_s(strip_s.Color(0, 255, 255), 15, 16, 10); //Air
  segment_s(strip_s.Color(0, 255, 255), 86, 102, 10); //Air
  segment_s(strip_s.Color(0, 255, 255), 23, 24, 10); //Air
  strip_s.show();
}

void gesto1_on() {
  strip_g.clear();
  segment_g(strip_g.Color(s_on, s_on, s_on), side_a_1, side_a_1 + 5, 0);
  segment_g(strip_g.Color(s_off, s_off, s_off), side_a_2, side_a_2 + 5, 0);
  segment_g(strip_g.Color(s_on, s_on, s_on), side_b_1, side_b_1 + 5, 0);
  segment_g(strip_g.Color(s_off, s_off, s_off), side_b_2, side_b_2 + 5, 0);
  strip_g.show();
}

void gesto1_off() {
  strip_g.clear();
  segment_g(strip_g.Color(s_off, s_off, s_off), side_a_1, side_a_1 + 5, 0);
  segment_g(strip_g.Color(s_on, s_on, s_on), side_a_2, side_a_2 + 5, 0);
  segment_g(strip_g.Color(s_off, s_off, s_off), side_b_1, side_b_1 + 5, 0);
  segment_g(strip_g.Color(s_on, s_on, s_on), side_b_2, side_b_2 + 5, 0);
  strip_g.show();
}

void fijo() {
  segment_g(strip_g.Color(255, 255, 255), side_a_1, side_a_1 + 5, 0);
  segment_g(strip_g.Color(255, 255, 255), side_a_2, side_a_2 + 5, 0);
  segment_g(strip_g.Color(255, 255, 255), side_b_1, side_b_1 + 5, 0);
  segment_g(strip_g.Color(255, 255, 255), side_b_2, side_b_2 + 5, 0);
  delay(1000);
}

void cambia_sta_int() {
  int_state = !int_state;
}


void theaterChase_c(uint32_t color, int wait) {
  for(int a=0; a<10; a++) {  // Repeat 10 times...
    for(int b=0; b<3; b++) { //  'b' counts from 0 to 2...
      strip_c.clear();         //   Set all pixels in RAM to 0 (off)
      strip_g.clear();
      strip_s.clear();
      // 'c' counts up from 'b' to end of strip in steps of 3...
      for(int c=b; c<strip_c.numPixels(); c += 3) {
        strip_c.setPixelColor(c, color); // Set pixel 'c' to value 'color'
        strip_g.setPixelColor(c, color);
        strip_s.setPixelColor(c, color);
      }
      strip_c.show(); // Update strip with new contents
      strip_g.show();
      strip_s.show();
      delay(wait);  // Pause for a moment
    }
  }
}
