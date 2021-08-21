using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleGrid : MonoBehaviour
{
    public Material triMat;

    public GameObject pointPF;

    GameObject pointsHolder;
    GameObject trianglesHolder;

    Shader pointShader;
    
    List<GameObject> pointList = new List<GameObject>();

    public int width =3 ;
    public int height=3;
    public int depth=3;

    public float threshold;

    // Texture3D texture;

    //interpolation
    // static int[2] a = {0, 2}; o ponto "a" esta entre vertice A e C

    // static float[] a = {0f, 0.5f, 0f};
    // static float[] b = {0.5f, 0f, 0f};
    // static float[] c = {0.5f, 1f, 0f};
    // static float[] d = {1f, 0.5f, 0f};
    // static float[] e = {0f, 0f, 0.5f};
    // static float[] f = {1f, 0f, 0.5f};
    // static float[] g = {0f, 0.5f, 1f};
    // static float[] h = {1f, 0.5f, 1f};
    // static float[] i = {0.5f, 1f, 1f};
    // static float[] j = {0.5f, 0f, 1f};
    // static float[] k = {0f, 1f, 0.5f};
    // static float[] l = {1f, 1f, 0.5f};
    
    static int[] a = {0, 2}; // A C
    static int[] b = {0, 1}; // A B
    static int[] c = {2, 3}; // C D
    static int[] d = {1, 3}; // B D
    static int[] e = {0, 4}; // A E
    static int[] f = {1, 5}; // B F
    static int[] g = {4, 6}; // E G
    static int[] h = {5, 7}; // F H
    static int[] i = {6, 7}; // G H
    static int[] j = {4, 5}; // E F
    static int[] k = {2, 6}; // C G
    static int[] l = {3, 7}; // D H
    
    int[][,] map = new int[][,]{
        new int[,]{{0, 0, 0, 0, 0}, {0, 0, 0, 0, 0}, {0, 0, 0, 0, 0}, {0, 0, 0, 0, 0}, {0, 0, 0, 0, 0}}, 
        new int[,]{{0, 0, 0, 0, 0}, {0, 0, 0, 0, 0}, {0, 0, 0, 0, 0}, {0, 0, 0, 0, 0}, {0, 0, 0, 0, 0}}, 
        new int[,]{{0, 0, 0, 0, 0}, {0, 0, 1, 0, 0}, {0, 1, 0, 0, 0}, {0, 0, 0, 0, 0}, {0, 0, 0, 0, 0}}, 
        new int[,]{{0, 0, 0, 0, 0}, {0, 1, 1, 0, 0}, {0, 1, 0, 0, 0}, {0, 0, 0, 0, 0}, {0, 0, 0, 0, 0}}, 
        new int[,]{{0, 0, 0, 0, 0}, {0, 0, 0, 0, 0}, {0, 0, 0, 0, 0}, {0, 0, 0, 0, 0}, {0, 0, 0, 0, 0}}
    };
    // all 5 active 2 deactive points cases
    // o -> off
    // x -> on
    // ambiguous case number 246              ambiguous case number 118               ambiguous case number 214
    // x-------x                            x-------o                               x-------x
    // |       |                            |       |                               |       |
    // |   x-------o                        |   x-------o                           |   x-------o
    // |   |   |   |                        |   |   |   |                           |   |   |   |    
    // x-------x   |                        x-------x   |                           x-------o   |
    //     |       |                            |       |                               |       |
    //     o-------x                            o-------x                               o-------x
    // 11110110 . 246                       01110110 - 118                          11010110 - 214 
    // 01111101 . 125                       00111101 - 61                           10011110 - 158
    // 10011111 . 159                       10011011 - 155                          01101101 - 109 
    // 11101011 . 235                       11100011 - 227                          11101001 - 233
    // 11101101 . 237                       10111001 - 185                          01111001 - 121
    // 11010111 . 215                       11011001 - 217                          10010111 - 151 
    // 10111110 . 190                       10101101 - 173                          10110110 - 182 
    // 11011110 . 222                       11011010 - 218                          01101011 - 107
    // 10110111 - 183                       00111110 - 62                           
    //                                      11010011 - 211 
    //                                      01101110 - 110 
    //                                      01111010 - 122 
    //                                      11000111 - 199
    //                                      11001011 - 203
    //                                      01111100 - 124 
    //                                      10111100 - 188
    //                                      01100111 - 103
    //                                      10100111 - 167
    //                                      10011101 - 157 
    //                                      11100101 - 229 
    //                                      01011110 - 94 
    //11110110
    public int[][][] triangulationList = new int[][][]{
        // new int[][]{e, b, a, j, g, k, j, k, c, j, c, f, f, c, d},
        new int[][]{},                                    // 00000000  HGFEDCBA
        new int[][]{b, a, e},                             // 00000001 1
        new int[][]{f, d, b},                             // 00000010 2
        new int[][]{a, e, f, f, d, a},                    // 00000011 3
        new int[][]{c, k, a},                             // 00000100 4
        new int[][]{c, k, e, b, c, e},                    // 00000101 5
        new int[][]{f, d, b, c, k, a},                    // 00000110 6 
        new int[][]{e, f, k, f, c, k, f, d, c},           // 00000111 7
        new int[][]{l, c, d},                             // 00001000 8
        new int[][]{b, a, e, l, c, d},                    // 00001001 9
        new int[][]{l, c, f, c, b, f},                    // 00001010 10
        new int[][]{l, e, f, l, c, e, c, a, e},           // 00001011 11
        new int[][]{k, a, l, a, d, l},                    // 00001100 12
        new int[][]{l, k, e, l, e, d, d, e, b},           // 00001101 13
        new int[][]{l, k, f, k, a, f, f, a, b},           // 00001110 14
        new int[][]{l, k, f, k, e, f},                    // 00001111 15
        new int[][]{e, g, j},                             // 00010000 16
        new int[][]{a, g, j, b, a, j},                    // 00010001 17
        new int[][]{e, g, j, f, d, b},                    // 00010010 18
        new int[][]{a, g, d, d, g, j, d, j, f},           // 00010011 19
        new int[][]{e, g, j, c, k, a},                    // 00010100 20
        new int[][]{b, c, j, j, c, k, k, g, j},           // 00010101 21
        new int[][]{e, g, j, c, k, a, d, b, f},           // 00010110 22
        new int[][]{c, k, j, f, c, j, d, c, f, k, g, j},  // 00010111 23
        new int[][]{e, g, j, l, c, d},                    // 00011000
        new int[][]{a, g, j, b, a, j, l, c, d},           // 00011001
        new int[][]{e, g, j, l, c, f, c, b, f},           // 00011010
        new int[][]{f, c, g, f, g, j, a, g, c, l, c, f},  // 00011011
        new int[][]{e, g, j, k, a, l, a, d, l},           // 00011100
        new int[][]{l, k, g, b, l, g, j, b, g, d, l, b},  // 00011101
        new int[][]{l, k, f, k, a, f, a, b, f, e, g, j},  // 00011110
        new int[][]{l, k, f, k, g, f, f, g, j},           // 00011111
        new int[][]{j, h, f},                             // 00100000 32
        new int[][]{a, e, b, j, h, f},                    // 00100001
        new int[][]{h, d, j, b, j, d},                    // 00100010
        new int[][]{j, a, e, j, h, a, h, d, a},           // 00100011
        new int[][]{j, h, f, c, k, a},                    // 00100100
        new int[][]{j, h, f, c, k, b, k, e, b},           // 00100101
        new int[][]{j, h, b, h, d, b, c, k, a},           // 00100110
        new int[][]{j, h, e, e, h, c, h, d, c, k, e, c},  // 00100111
        new int[][]{j, h, f, l, c, d},                    // 00101000
        new int[][]{j, h, f, l, c, d, a, e, b},           // 00101001
        new int[][]{c, b, j, h, c, j, h, l, c},           // 00101010
        new int[][]{h, c, j, c, a, j, a, e, j, l, c, h},  // 00101011
        new int[][]{k, a, l, a, d, l, j, h, f},           // 00101100
        new int[][]{h, f, j, l, k, e, l, e, d, d, e, b},  // 00101101
        new int[][]{j, h, l, j, l, a, j, a, b, l, k, a},  // 00101110
        new int[][]{l, k, e, h, l, e, h, e, j},           // 00101111
        new int[][]{g, h, e, e, h, f},                    // 00110000
        new int[][]{g, h, a, a, h, f, a, f, b},           // 00110001
        new int[][]{g, h, d, g, d, e, e, d, b},           // 00110010
        new int[][]{g, h, a, a, h, d},                    // 00110011
        new int[][]{g, h, e, h, f, e, c, k, a},           // 00110100
        new int[][]{k, g, c, c, g, f, c, f, b, g, h, f},  // 00110101
        new int[][]{g, h, d, g, d, e, e, d, b, c, k, a},  // 00110110 54
        new int[][]{g, h, d, c, g, d, c, k, g},           // 00110111 55
        new int[][]{g, h, e, h, f, e, l, c, d},           // 00111000 56
        new int[][]{g, h, a, a, h, b, b, h, f, l, c, d},  // 00111001 57
        new int[][]{c, b, e, h, c, e, h, l, c, g, h, e},  // 00111010 58
        new int[][]{a, g, h, h, c, a, h, l, c},           // 00111011 59
        new int[][]{g, h, e, e, h, f, k, a, l, l, a, d},  // 00111100 60
        // new int[][]{k, g, h, l, k, h, b, d, f},           // 00111101 61
        new int[][]{b, d, l, b, l, f, f, g, h, f, l, g, l, k, g}, // 00111101 61
        // new int[][]{k, g, h, l, k, h, e, a, b},           // 00111110 62
        new int[][]{e, k, b, k, a, b, l, k, h, h, k, e, e, g, h}, // 00111110 62
        new int[][]{l, k, h, k, g, h},                    // 00111111 = 63
        new int[][]{k, i, g},                             // 01000000 = 64
        new int[][]{k, i, g, a, e, b},                    // 01000001
        new int[][]{k, i, g, f, d, b},                    // 01000010
        new int[][]{f, a, e, d, a, f, k, i, g},           // 01000011
        new int[][]{c, i, g, c, g, a},                    // 01000100
        new int[][]{c, i, b, b, i, g, b, g, e},           // 01000101
        new int[][]{f, d, b, c, i, a, a, i, g},           // 01000110
        new int[][]{i, g, e, i, e, d, i, d, c, d, e, f},  // 01000111
        new int[][]{k, i, g, l, c, d},                    // 01001000
        new int[][]{k, i, g, l, c, d, a, e, b},           // 01001001
        new int[][]{l, c, b, l, b, f, k, i, g},           // 01001010
        new int[][]{k, i, g, l, e, f, l, a, e, l, c, a},  // 01001011
        new int[][]{a, d, g, g, d, i, d, l, i},           // 01001100
        new int[][]{i, g, b, d, i, b, l, i, d, b, g, e},  // 01001101
        new int[][]{g, a, i, i, a, f, a, b, f, f, l, i},  // 01001110
        new int[][]{l, e, f, l, g, e, l, i, g},           // 01001111
        new int[][]{k, i, e, e, i, j},                    // 01010000
        new int[][]{b, i, j, b, k, i, b, a, k},           // 01010001
        new int[][]{k, i, e, e, i, j, f, d, b},           // 01010010
        new int[][]{f, d, j, d, a, k, k, j, d, i, j, k},  // 01010011
        new int[][]{c, i, j, a, c, j, e, a, j},           // 01010100
        new int[][]{i, j, b, c, i, b},                    // 01010101
        new int[][]{c, i, j, a, c, j, e, a, j, f, d, b},  // 01010110
        new int[][]{i, j, c, c, j, f, d, c, f},           // 01010111
        new int[][]{k, i, e, e, i, j, c, d, l},           // 01011000
        new int[][]{i, j, b, k, i, b, a, k, b, l, c, d},  // 01011001
        new int[][]{k, i, j, e, k, j, c, b, f, f, l, c},  // 01011010
        // new int[][]{k, c, a, l, i, f, i, j, f},           // 01011011 91
        new int[][]{a, k, i, a, i, c, c, f, l, c, i, f, i, j, f}, // 01011011 91                
        new int[][]{i, j, l, j, a, l, j, e, a, a, d, l},  // 01011100 92
        new int[][]{b, i, j, b, l, i, b, d, l},           // 01011101 93
        // new int[][]{l, i, f, f, i, j, e, a, b},           // 01011110 94
        new int[][]{e, a, j, j, a, b, l, i, j, b, l, j, f, l, b}, // 01011110 94
        new int[][]{l, i, f, f, i, j},                    // 01011111 95
        new int[][]{k, i, g, j, h, f},                    // 01100000 96
        new int[][]{k, i, g, j, h, f, a, e, b},           // 01100001 97
        new int[][]{k, i, g, h, d, b, j ,h, b},           // 01100010 98
        new int[][]{k, i, g, a, h, d, a, e, h, e, j, h},  // 01100011 99
        new int[][]{j, h, f, c, i, a, a, i, g},           // 01100100 = 100
        new int[][]{j, h, f, c, i, b, i, g, b, g, e, b},  // 01100101 101
        new int[][]{c, i, a, a, i, g, h, b, j, h, d, b},  // 01100110 102
        // new int[][]{j, g, e, c, i, h, d, c, h},           // 01100111 103
        new int[][]{c, h, d, h, g, e, e, j, h, h, c, g, c, i, g}, // 01100111 103
        new int[][]{j, h, f, l, c, d, k, i, g},           // 01101000 104
        new int[][]{j, h, f, l, c, d, k, i, g, a, e, b},  // 01101001 105
        new int[][]{k, i, g, c, b, j, h, c, j, h, l, c},  // 01101010 106
        // new int[][]{l, i, h, c, k, a, g, e, j},           // 01101011 107
        new int[][]{g, k, i, c, a, e, c, e, j, c, j, l, j, h, l}, // 01101011 107
        new int[][]{j, h, f, a, d, g, g, d, i, i, d, l},  // 01101100 108
        // new int[][]{g, e, j, l, i, h, b, d, f},           // 01101101 109
        new int[][]{j, h, f, l, b, d, l, e, b, i, e, l, g, e, i}, // 01101101 109
        // new int[][]{l, i, h, g, a, j, a, b, j},           // 01101110
        new int[][]{j, h, l, l, i, j, j, a, b, a, i, g, i, a, j}, // 01101110
        // new int[][]{g, e, j, l, i, h},                    // 01101111
        new int[][]{e, g, i, e, i, j, j, i, l, l, h, i},  // 01101111
        new int[][]{k, f, e, k, i, f, i, h, f},           // 01110000
        new int[][]{k, h, a, k, i, h, a, h, f, b, a, f},  // 01110001
        new int[][]{h, d, b, b, k, h, k, i, h, k ,b, e},  // 01110010
        new int[][]{a, h, d, k, h, a, i, h, k},           // 01110011
        new int[][]{e, a, f, a, c, f, f, c, h, c, i, h},  // 01110100
        new int[][]{b, c, i, i, h, b, b, h, f},           // 01110101
        // new int[][]{e, a, b, c, i, h, d, c, h},           // 01110110
        new int[][]{e, a, c, e, c, b, b, h, d, b, c, h, c, i, h}, // 01110110
        new int[][]{c, i, h, d, c, h},                    // 01110111
        new int[][]{c, d, l, e, k, f, k, i, f, i, h, f},  // 01111000
        // new int[][]{k, c, a, l, i, h, d, f, b},           // 01111001
        new int[][]{b, a, k, k, i, b, b, i, f, i, h, f, c, d, l}, // 01111001
        // new int[][]{l, i, h, k, c, b, k, b, e},           // 01111010
        new int[][]{e, c, b, e, k, i, e, i, c, c, i, h, h, l, c}, // 01111010
        // new int[][]{k, c, a, l, i, h},                    // 01111011
        new int[][]{a, k, i, a, i, c, c, i, h, h, l, i},  // 01111011
        // new int[][]{e, a, f, a, d, f, l, i, h},           // 01111100
        new int[][]{e, a, f, f, l, i, i, h, f, a, l, f, l, a, d}, // 01111100
        // new int[][]{l, i, h, b, d, f},                    // 01111101 125
        new int[][]{b, d, l, b, l, f, f, l, i, i, h, f},  // 01111101 125
        new int[][]{e, a, b, l, i, h},                    // 01111110 126
        new int[][]{l, i, h},                             // 01111111 127
        new int[][]{i, l, h},                             // 10000000 128 - OK
        new int[][]{i, l, h, a, e, b},                    // 10000001 129
        new int[][]{i, l, h, f, d, b},                    // 10000010 130
        new int[][]{i, l, h, a, e, f, f, d, a},           // 10000011 131
        new int[][]{i, l, h, c, k, a},                    // 10000100 132
        new int[][]{i, l, h, c, k, e, b, c, e},           // 10000101 133
        new int[][]{i, l, h, c, k, a, f, d, b},           // 10000110 134
        new int[][]{i, l, h, k, e, f, c, k, f, c, f, d},  // 10000111 135
        new int[][]{i, c, d, d, h, i},                    // 10001000 136
        new int[][]{i, c, d, d, h, i, a, e, b},           // 10001001 137
        new int[][]{i, c, b, i, b, f, i, f, h},           // 10001010 138
        new int[][]{e, f, a, f, i, a, f, h, i, a, i, c},  // 10001011 139
        new int[][]{h, a, d, k, a, i, i, a, h},           // 10001100 140
        new int[][]{e, i, k, e, d, i, d, h, i, e, b, d},  // 10001101 141
        new int[][]{k, a, b, i, k ,b, f, i, b, f, h, i},  // 10001110 142
        new int[][]{k, e, f, i, k, f, f, h, i},           // 10001111 143
        new int[][]{e, g, j, i, l, h},                    // 10010000 144
        new int[][]{a, g, j, a, j, b, i, l, h},           // 10010001 145
        new int[][]{e, g, j, i, l, h, f, d, b},           // 10010010 146
        new int[][]{i, l, h, a, g, d, g, j, d, j, f, d},  // 10010011 147
        new int[][]{i, l, h, e, g, j, c, k, a},           // 10010100 148
        new int[][]{b, c, j, c, k, j, k, g, j, i, l, h},  // 10010101 149
        new int[][]{e, g, j, i, l, h, c, k, a, f, d, b},  // 10010110 150
        // new int[][]{h, j, f, c, l, d, i, k, g},           // 10010111 151
        new int[][]{f, d, c, f, c, j, k, j, c, k, g, j, i, l, h},// 10010111 151
        new int[][]{e, g, j, h, i, c, h, c, d},           // 10011000 152
        new int[][]{h, i, c, h, c, d, a, g, j, b, a, j},  // 10011001 153
        new int[][]{e, g, j, i, c, b, h, i, b, h, b, f},  // 10011010 154
        // new int[][]{g, i, a, i, c, a, h, j, f},           // 10011011 155
        new int[][]{f, h, i, f, i, j, j, a, g, j, i, a, i, c, a}, // 10011011 155
        new int[][]{e, g, j, a, d, h, k, a, h, i, k, h},  // 10011100 156
        // new int[][]{d, j, b, d, h, j, i, k, g},           // 10011101 157
        new int[][]{b, d, j, k, g, j, i, k, j, d, i, j, d, l, i}, // 10011101 157
        // new int[][]{f, h, j, e, a, b, i, k, g},           // 10011110 158
        new int[][]{e, g, j, f, l, i, f, i, b, b, i, k, a, b, k},  // 10011110 158
        // new int[][]{f, h, j, i, k, g},                    // 10011111 159
        new int[][]{f, h, i, f, i, j, j, i, k, k, g, j},  // 10011111 159
        new int[][]{j, i, l, j, l, f},                    // 10100000 160
        new int[][]{j, i, l, j, l, f, a, e, b},           // 10100001 161
        new int[][]{j, i, b, i, l, b, b, l, d},           // 10100010 162
        new int[][]{i, e, j, i, l, d, d, a, e, i, d, e},  // 10100011 163
        new int[][]{c, k, a, i, l, f, i, f, j},           // 10100100 164
        new int[][]{i, l, f, i, f, j, c, k, e, c, e, b},  // 10100101 165
        new int[][]{c, k, a, i, b, j, i, l, b, l, d, b},  // 10100110 166
        // new int[][]{c, l, d, i, k, e, i, e, j},           // 10100111 167
        new int[][]{i, l, d, d, c, i, k, e, c, i, c, e, j, i, e}, // 10100111 167
        new int[][]{i, c, j, j, c, f, f, c, d},           // 10101000 168
        new int[][]{i, c, j, j, c, f, f, c, d, a, e, b},  // 10101001 169
        new int[][]{j, i, b, i, c, b},                    // 10101010 170
        new int[][]{i, c, j, j, c, a, j, a, e},           // 10101011 171
        new int[][]{f, a, d, i, k, a, j, i, f, f, i, a},  // 10101100 172
        // new int[][]{b, d, f, i, k, e, i, e, j},           // 10101101 173
        new int[][]{d, f, j, d, j, b, i, k, j, k, e, b, k, b, j}, // 10101101 173
        new int[][]{i, b, j, i, k, b, k, a, b},           // 10101110 174
        new int[][]{i, k, e, i, e, j},                    // 10101111 175
        new int[][]{e, l, f, g, l, e, g, i, l},           // 10110000 176
        new int[][]{b, l, f, g, i, l, g, l, a, a, l, b},  // 10110001 177
        new int[][]{b, e, g, b, g, i, d, b, i, i, l, d},  // 10110010 178
        new int[][]{d, a, g, d, g, i, i, l, d},           // 10110011 179
        new int[][]{c, k, a, e, l, f, e, g, l, g, i, l},  // 10110100 180
        // new int[][]{c, l, b, b, l, f, i, k, g},           // 10110101 181
        new int[][]{b, c, f, i, c, g, c, k, g, f, c, i, i, l, f}, // 10110101 181
        // new int[][]{e, a, b, c, l, d, i, k, g},           // 10110110 182
        new int[][]{c, k, a, g, i, e, e, i, b, b, i, l, b, l, d}, // 10110110 182
        new int[][]{d, i, l, c, i, d, i, c, g, k, g, c},  // 10110111 183
        new int[][]{d, f, e, d, e, i, e, g, i, d, i, c},  // 10111000 184
        // new int[][]{b, d, f, i, c, a, a, g, i},           // 10111001 185
        new int[][]{d, f, c, c, f, b, g, b, a, c, b, g, i, c, g}, // 10111001 185
        new int[][]{i, c, b, g, i, b, g, b, e},           // 10111010 186
        new int[][]{i, c, a, a, g, i},                    // 10111011 187
        // new int[][]{i, k, g, e, a, d, f, e, d},           // 10111100 188
        new int[][]{d, k, a, e, d, f, e, k, d, i, k, e, g, i, e}, // 10111100 188
        new int[][]{i, k, g, b, d, f},                    // 10111101 189
        // new int[][]{i, k, g, e, a, b},                    // 10111110 190
        new int[][]{a, b, k, k, b, e, k, e, i, g, i, e},  // 10111110 190
        new int[][]{i, k, g},                             // 10111111 191
        new int[][]{g, k, l, l, h, g},                    // 11000000 192
        new int[][]{g, k, l, l, h, g, a, e, b},           // 11000001 193
        new int[][]{g, k, l, l, h, g, f, d, b},           // 11000010 194
        new int[][]{g, k, l, l, h, g, a, e, f, f, d, a},  // 11000011 195
        new int[][]{g, a, h, a, c, h, h, c, l},           // 11000100 196
        new int[][]{b, c, e, c, l, h, e, c, h, e, h, g},  // 11000101 197
        new int[][]{f, d, b, h, g, a, a, c, h, c, l, h},  // 11000110 198
        // new int[][]{c, l, d, f, h, g, g, e, f},           // 11000111 199
        new int[][]{d, c, f, c, l, f, f, g, e, l, g, f, l, h, g}, // 11000111 199
        new int[][]{d, h, g, g, k, d, k, c, d},           // 11001000 200
        new int[][]{a, e, b, h, g, d, g, k, d, k, c, d},  // 11001001 201
        new int[][]{h, g, f, g, k, c, g, c, f, c, b, f},  // 11001010 202
        // new int[][]{k, c, a, h, g, e, f, h, e},           // 11001011 203
        new int[][]{k, c, e, c, a, e, h, e, f, g, k, h, k, e, h}, // 11001011 203
        new int[][]{g, a, h, d, h, a},                    // 11001100 204
        new int[][]{d, g, e, h, g, d, d, e, b},           // 11001101 205
        new int[][]{a, h, g, a, f, h, a, b, f},           // 11001110 206
        new int[][]{g, e, h, e, f, h},                    // 11001111 207
        new int[][]{e, k, l, e, l, j, j, l, h},           // 11010000 208
        new int[][]{k, l, a, l, j, a, j, b, a, l, h, j},  // 11010001 209
        new int[][]{e, k, l, e, l, j, j, l, h, f, d, b},  // 11010010 210
        // new int[][]{f, h, j, k, l, a, d, a, l},           // 11010011 211
        new int[][]{a, f, d, f, a, l, a, k, l, l, h, j, f, l, j}, // 11010011 211
        new int[][]{j, a, c, c, h, j, j, e, a, c, l, h},  // 11010100 212
        new int[][]{c, j, b, j, c, h, c, l, h},           // 11010101 213
        // new int[][]{e, a, b, f, h, j, c, l, d},           // 11010110 214
        new int[][]{f, d, b, e, a, j, a, h, j, a, c, h, h, c, l}, // 11010110 214
        // new int[][]{f, h, j, c, l, d},                    // 11010111 215
        new int[][]{h, j, l, l, j, f, l, f, c, d, c, f},  // 11010111 215
        new int[][]{e, k, j, k, d, j, k, c, d, j, d, h},  // 11011000 216
        // new int[][]{k, c, a, d, h, b, h, j, b},           // 11011001 217
        new int[][]{k, c, d, k, d, a, a, j, b, a, d, j, d, h, j}, // 11011001 217
        // new int[][]{k, c, b, k, b, e, f, h, j},           // 11011010 218
        new int[][]{h, b, f, h, j, b, j, c, b, k, c, e, e, c, j}, // 11011010 218
        new int[][]{k, c, a, f, h, j},                    // 11011011 219
        new int[][]{a, d, h, a, h, e, h, j, e},           // 11011100 220
        new int[][]{b, d, h, j, b, h},                    // 11011101 221
        // new int[][]{e, a, b, f, h, j},                    // 11011110 222
        new int[][]{f, h, b, b, h, j, b, j, a, e, a, j},  // 11011110 222
        new int[][]{f, h, j},                             // 11011111 223
        new int[][]{k, l, f, g, k, f, g, f, j},           // 11100000 224
        new int[][]{k, l, f, g, k, f, g, f, j, a, e, b},  // 11100001 225
        new int[][]{b, j, d, j, g, k, d, j, k, d, k, l},  // 11100010 226
        // new int[][]{j, g, e, k, a, l, l, a, d},           // 11100011 227
        new int[][]{j, g, k, j, k, e, e, d, a, e, k, d, k, l, d}, // 11100011 227
        new int[][]{g, a, j, a, c, l, a, l, j, j, l, f},  // 11100100 228
        // new int[][]{j, g, e, b, c, l, b, l, f},           // 11100101 229
        new int[][]{b, c, j, j, c, f, c, l, f, e, b, g, g, b, j}, // 11100101 229
        // new int[][]{c, l, d, g, a, j, j, a, b},           // 11100110 230
        new int[][]{l, d, b, l, b, c, c, g, a, c, b, g, b, j, g}, // 11100110 230
        new int[][]{j, g, e, c, l, d},                    // 11100111 231
        new int[][]{j, g, k, j, k, c, j, c, f, f, c, d},  // 11101000 232
        // new int[][]{j, g, e, k, c, a,b, d, f},            // 11101001 233
        new int[][]{e, b, a, g, l, k, j, k, c, j, c, f, f, c, d}, // 11101001 233
        new int[][]{j, c, b, j, k, c, g, k, j},           // 11101010 234
        // new int[][]{k, c, a, j, g, e},                    // 11101011 235
        new int[][]{j, g, k, j, k, e, e, k, c, c, a, e},  // 11101011 235
        new int[][]{g, a, d, g, d, j, j, d, f},           // 11101100 236
        // new int[][]{g, e, j, d, f, b},                    // 11101101 237
        new int[][]{g, e, b, g, b, j, j, b, d, d, f, j},  // 11101101 237
        new int[][]{g, a, j, j, a, b},                    // 11101110 238
        new int[][]{g, e, j},                             // 11101111 239
        new int[][]{f, e, k, k, l, f},                    // 11110000 240
        new int[][]{k, l, f, a, k, f, f, b, a},           // 11110001 241
        new int[][]{e, k, l, e, l, b, b, l, d},           // 11110010 242
        new int[][]{a, k, l, a, l, d},                    // 11110011 243
        new int[][]{e, l, f, e, a, l, a, c, l},           // 11110100 244
        new int[][]{c, l, b, b, l, f},                    // 11110101 245
        // new int[][]{e, a, b, c, l, d},                    // 11110110 246
        new int[][]{e, a, c, e, c, b, b, c, l, l, d, b},  // 11110110 247
        new int[][]{c, l, d},                             // 11110111 248
        new int[][]{e, k, f, k, c, f, c, d, f},           // 11111000 249
        new int[][]{c, d, f, b, c, f, k, c, b, b, a, k},  // 11111001
        new int[][]{e, k, c, e, c, b},                    // 11111010
        new int[][]{k, c, a},                             // 11111011
        new int[][]{e, a, d, e, d, f},                    // 11111100
        new int[][]{b, d, f},                             // 11111101
        new int[][]{e, a, b},                             // 11111110
        new int[][]{} // 11111111
    };

    GameObject FindEdgePoint(int baseX, int baseY, int baseZ, int edgeNum){
        // when using this for debugSquare function, you need to set
        // width, height and depth to 2 to work properly!!!!!!!!!!!!!!!
        int zz = 0;
        int yy = 0;
        int xx = 0;
        if (edgeNum >= 4){
            zz = 1;
            edgeNum -= 4;
        }
        if (edgeNum >= 2){
            yy = 1;
            edgeNum -= 2;
        }
        if (edgeNum >= 1){
            xx = 1;
            edgeNum -= 1;
        }
        GameObject a = pointList[(baseX + xx) + ((baseY + yy) * width) + ((baseZ + zz) * width * height)];
        return a;
    }

    float VertexPositionInterpolated(float valueA, float valueB){
        if (valueA < 0){
            valueA *= -1;
        }
        if (valueB > 0){
            valueB *= -1;
        }
        float value =  (0 - valueA) / (valueB - valueA); // interpolation
        // float value = 0.5f; fixed value
        return value;
    }

    void March(int x, int y, int z){
        GameObject cubeMesh = new GameObject("Cube Mesh");
        cubeMesh.transform.parent = trianglesHolder.transform;
        int[][] triConfig = triangulationList[
            (pointList[x + (y * width) + (z * width * height)].GetComponent<State>().isOn == true ? 1 : 0) +
            (pointList[(x+1) + (y * width) + (z * width * height)].GetComponent<State>().isOn == true ? 2 : 0) +
            (pointList[x + ((y+1) * width) + (z * width * height)].GetComponent<State>().isOn == true ? 4 : 0) +
            (pointList[(x+1) + ((y+1) * width) + (z * width * height)].GetComponent<State>().isOn == true ? 8 : 0) +
            (pointList[x + (y * width) + ((z+1) * width * height)].GetComponent<State>().isOn == true ? 16 : 0) +
            (pointList[(x+1) + (y * width) + ((z+1) * width * height)].GetComponent<State>().isOn == true ? 32 : 0) +
            (pointList[x + ((y+1) * width) + ((z+1) * width * height)].GetComponent<State>().isOn == true ? 64 : 0) +
            (pointList[(x+1) + ((y+1) * width) + ((z+1) * width * height)].GetComponent<State>().isOn == true ? 128 : 0)
        ];
        int config = (pointList[x + (y * width) + (z * width * height)].GetComponent<State>().isOn == true ? 1 : 0) +
            (pointList[(x+1) + (y * width) + (z * width * height)].GetComponent<State>().isOn == true ? 2 : 0) +
            (pointList[x + ((y+1) * width) + (z * width * height)].GetComponent<State>().isOn == true ? 4 : 0) +
            (pointList[(x+1) + ((y+1) * width) + (z * width * height)].GetComponent<State>().isOn == true ? 8 : 0) +
            (pointList[x + (y * width) + ((z+1) * width * height)].GetComponent<State>().isOn == true ? 16 : 0) +
            (pointList[(x+1) + (y * width) + ((z+1) * width * height)].GetComponent<State>().isOn == true ? 32 : 0) +
            (pointList[x + ((y+1) * width) + ((z+1) * width * height)].GetComponent<State>().isOn == true ? 64 : 0) +
            (pointList[(x+1) + ((y+1) * width) + ((z+1) * width * height)].GetComponent<State>().isOn == true ? 128 : 0);
        
        // pointList[x + (y * width) + (z * width * height)].GetComponent<State>().triangNumber = config;
        
        // Vector3 pivotPos = (pointList[x + (y * width) + (z * width * height)].transform.position);

        cubeMesh.AddComponent<State>();
        cubeMesh.GetComponent<State>().value = config;

        // Debug.Log(triConfig.Length);
        for (int iter = 0; iter < triConfig.Length/3; iter++){
            GameObject triangleHolder = new GameObject("Mesh Holder");
            triangleHolder.transform.parent = cubeMesh.transform;
            triangleHolder.AddComponent<MeshFilter>();
            triangleHolder.AddComponent<MeshRenderer>();
            triangleHolder.GetComponent<MeshRenderer>().material = triMat;
            Mesh mesh = triangleHolder.GetComponent<MeshFilter>().mesh;

            // interpolation
            // you need to get the value at points B and A
            // get the medium value between them and chose the 
            // intersection point to be this new calculated value base on the 2 point
        
             
            mesh.Clear();
            mesh.vertices = new Vector3[]{
                // interpolation
                // offset = (vertice A.xyz - Vertice B.xyz) * 0.5; mesma coisa q ja esta sendo feito
                // vertice vai na posicao Vertice B.xyz + offset
                // (vertice A.xyz - Vertice B.xyz) -> acho onde sera colocado o vertice do triangulo
                // dps disso, ver o valor do ponto A e B, e calcular a media para colocar
                // o vertice do triangulo
                

                (FindEdgePoint(x, y, z, triConfig[0+(3*iter)][0]).transform.position + 
                ((FindEdgePoint(x, y, z, triConfig[0+(3*iter)][1]).transform.position - 
                FindEdgePoint(x, y, z, triConfig[0+(3*iter)][0]).transform.position) *
                (float)VertexPositionInterpolated(FindEdgePoint(x, y, z, triConfig[0+(3*iter)][0]).GetComponent<State>().value, FindEdgePoint(x, y, z, triConfig[0+(3*iter)][1]).GetComponent<State>().value))),

                (FindEdgePoint(x, y, z, triConfig[1+(3*iter)][0]).transform.position + 
                ((FindEdgePoint(x, y, z, triConfig[1+(3*iter)][1]).transform.position - 
                FindEdgePoint(x, y, z, triConfig[1+(3*iter)][0]).transform.position) *
                (float)VertexPositionInterpolated(FindEdgePoint(x, y, z, triConfig[1+(3*iter)][0]).GetComponent<State>().value, FindEdgePoint(x, y, z, triConfig[1+(3*iter)][1]).GetComponent<State>().value))),
                
                (FindEdgePoint(x, y, z, triConfig[2+(3*iter)][0]).transform.position + 
                ((FindEdgePoint(x, y, z, triConfig[2+(3*iter)][1]).transform.position - 
                FindEdgePoint(x, y, z, triConfig[2+(3*iter)][0]).transform.position) *
                (float)VertexPositionInterpolated(FindEdgePoint(x, y, z, triConfig[2+(3*iter)][0]).GetComponent<State>().value, FindEdgePoint(x, y, z, triConfig[2+(3*iter)][1]).GetComponent<State>().value)))

            };
            mesh.uv = new Vector2[]{
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };
            mesh.triangles = new int[]{
                0, 1, 2
            };
            mesh.RecalculateNormals();
        }
    }

    void CreateGrid(int width, int height, int depth){
        float xoff = width/2;
        float yoff = height/2;
        
        // load a 3D texture
        Texture3D texture = Resources.Load<Texture3D>("Example3DTexture");

        pointsHolder = new GameObject("Points Holder");
        trianglesHolder = new GameObject("Triangles Holder");

        for (int k = 0; k < depth; k++){
            for (int j = 0; j < height; j++){
                for (int i = 0; i < width; i++){
                    GameObject p = Instantiate(pointPF, new Vector3(i-xoff, j-yoff, k), Quaternion.identity);
                    p.transform.parent = pointsHolder.transform;
                    p.GetComponentInChildren<Renderer>().material.shader = pointShader;
                    pointList.Add(p);
                    float val = 0;

                    // if the point is in the border its assigned a off state
                    
                    if (k == 0 || k == depth-1 || j == 0 || j == height-1 || i == 0 || i == width-1){
                        // val = threshold-1;
                        
                        val = Random.Range(-1f, 1f);
                    
                    }
                    else{
                        // load from texture
                        // val = texture.GetPixel(i, j, k).r;

                        // random value beetwen -8 and 8
                        val = Random.Range(-1f, 1f);
                        
                        // load from perlin noise
                        // val = Mathf.PerlinNoise(((float)i/width), ((float)j/height));
                        
                        // load from a pre-made array
                        // val = map[k][j, i];

                    }
                    p.GetComponent<State>().SetValue(val);
                    if (val >= threshold){
                        p.GetComponent<State>().isOn = true;
                        p.GetComponent<State>().SetColor(new Color(1f, 1f, 1f, 1f));
                    }
                    else{
                        p.GetComponent<State>().isOn = false;
                        p.GetComponent<State>().SetColor(new Color(0f, 0f, 0f, 1f));
                    }
                }
            }
        }
        for (int k = 0; k < depth-1; k++){
            for (int j = 0; j < height-1; j++){
                for (int i = 0; i < width-1; i++){
                    March(i, j, k);
                }
            }
        }
        CreateBorderTriangles(width, height, depth);
    }

    void CreateBorderTriangles(int width, int height, int depth){
        MarchingSquares mSqr = GetComponent<MarchingSquares>();
        mSqr.CreateBorders(width, height, depth, pointList);
    }

    void Start()
    {
        pointShader = Shader.Find("UI/Unlit/Transparent");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)){
            CreateGrid(width, height, depth);
            Debug.Log(pointList.Count);
        }
        if (Input.GetKeyDown(KeyCode.E)){
            foreach(GameObject point in pointList){
                point.GetComponent<State>().SetColor(new Color(1f, 1f, 1f, 0f));
            }
        }
        if (Input.GetKeyDown(KeyCode.R)){
            foreach(GameObject point in pointList){
                if (point.GetComponent<State>().value > threshold){
                    point.GetComponent<State>().SetColor(new Color(1f, 1f, 1f, 1f));
                }
                else{
                    point.GetComponent<State>().SetColor(new Color(0f, 0f, 0f, 1f));
                }
            }
        }
       
        
        if (Input.GetKeyDown(KeyCode.Z)){
            Destroy(trianglesHolder);
            Destroy(pointsHolder);
            pointList.Clear();
        }
    }
}
