/**
 * Gói Con Tim Làm Quà - Phiên Bản Dev
 * Full Interactive Pixel Art & Synchronized Code Debugger
 */

// --- CONFIG & STATE ---
const DURATION = 46.0;
let currentTime = 0.0;
let isPlaying = false;
let isAudioActive = false;
let isKaraokeEnabled = true;
let isMuted = false;
let customCrushName = localStorage.getItem('crushName');
if (!customCrushName || customCrushName === 'em') {
  customCrushName = 'Huyền';
  localStorage.setItem('crushName', 'Huyền');
}

let customBoySpeech = localStorage.getItem('boySpeech');
if (!customBoySpeech || customBoySpeech.includes('anh thích em')) {
  customBoySpeech = 'Hiếu thích Huyền';
  localStorage.setItem('boySpeech', 'Hiếu thích Huyền');
}

let customGirlSpeech = localStorage.getItem('girlSpeech');
if (!customGirlSpeech || customGirlSpeech.includes('ừ, em đồng ý')) {
  customGirlSpeech = 'ừ, Huyền đồng ý';
  localStorage.setItem('girlSpeech', 'ừ, Huyền đồng ý');
}

// DOM Elements
const canvas = document.getElementById('mainCanvas');
const ctx = canvas.getContext('2d');
const audio = document.getElementById('audioPlayer');
const playOverlay = document.getElementById('playOverlay');
const bigPlayBtn = document.getElementById('bigPlayBtn');
const btnPlayPause = document.getElementById('btnPlayPause');
const iconPlay = document.getElementById('iconPlay');
const iconPause = document.getElementById('iconPause');
const btnRestart = document.getElementById('btnRestart');
const btnMute = document.getElementById('btnMute');
const btnKaraokeToggle = document.getElementById('btnKaraokeToggle');
const btnCustomize = document.getElementById('btnCustomize');
const btnTogglePhone = document.getElementById('btnTogglePhone');
const btnFullscreen = document.getElementById('btnFullscreen');
const playerWrapper = document.getElementById('playerWrapper');
const timeDisplay = document.getElementById('timeDisplay');
const scrubContainer = document.getElementById('scrubContainer');
const scrubFill = document.getElementById('scrubFill');
const scrubHandle = document.getElementById('scrubHandle');

// Modal Elements
const modalBackdrop = document.getElementById('modalBackdrop');
const btnCloseModal = document.getElementById('btnCloseModal');
const btnResetDefault = document.getElementById('btnResetDefault');
const btnSaveCustomize = document.getElementById('btnSaveCustomize');
const inputCrush = document.getElementById('inputCrush');
const inputSpeech1 = document.getElementById('inputSpeech1');
const inputSpeech2 = document.getElementById('inputSpeech2');

// --- PIXEL COLOR PALETTE ---
const PAL = {
  skyTop: '#0a0e1a',
  skyBottom: '#121a2c',
  ground: '#0c120e',
  grass: '#223c26',
  moon: '#ebeef5',
  moonCrater: '#b4bac6',
  boyBody: '#ef5350',
  boyDark: '#c62828',
  boyCap: '#2196f3',
  girlBody: '#f06292',
  girlDark: '#c2185b',
  girlBow: '#ffd54f',
  girlBowCenter: '#ffb300',
  eyeBlack: '#141419',
  eyeWhite: '#ffffff',
  blush: '#ff80ab',
  heart: '#ff4081',
  heartShine: '#ffaac8',
  flowerPetal: '#ffd600',
  flowerCenter: '#a16207',
  flowerStem: '#4caf50',
  witheredPetal: '#b49628',
  witheredCenter: '#644614',
  witheredStem: '#466e3c',
  // Code Syntax Colors (VS Code)
  codeKeyword: '#569cd6',
  codeFunction: '#dcdcaa',
  codeVar: '#9cdcfe',
  codeString: '#ce9178',
  codeNumber: '#b5cea8',
  codePunct: '#d4d4d4',
  codeGutter: '#858585',
  codeArrow: '#4fc1ff',
  codeLineHl: 'rgba(255, 255, 255, 0.08)'
};

// --- SPRITES DEFINITIONS ---
function createOffscreenCanvas(w, h) {
  const c = document.createElement('canvas');
  c.width = w;
  c.height = h;
  return c;
}

function renderMatrix(rows, palette, pixelSize = 3) {
  const h = rows.length;
  const w = rows[0].length;
  const cvs = createOffscreenCanvas(w * pixelSize, h * pixelSize);
  const cctx = cvs.getContext('2d');
  cctx.imageSmoothingEnabled = false;

  for (let y = 0; y < h; y++) {
    const row = rows[y];
    for (let x = 0; x < w; x++) {
      const char = row[x];
      if (palette[char] && palette[char] !== 'transparent') {
        cctx.fillStyle = palette[char];
        cctx.fillRect(x * pixelSize, y * pixelSize, pixelSize, pixelSize);
      }
    }
  }
  return cvs;
}

// Generate Sprites
const boyPalette = {
  '.': 'transparent',
  'C': PAL.boyCap,
  'R': PAL.boyBody,
  'D': PAL.boyDark,
  'W': PAL.eyeWhite,
  'B': PAL.eyeBlack,
  'F': PAL.boyDark
};

const girlPalette = {
  '.': 'transparent',
  'Y': PAL.girlBow,
  'O': PAL.girlBowCenter,
  'P': PAL.girlBody,
  'M': PAL.girlDark,
  'W': PAL.eyeWhite,
  'R': PAL.eyeBlack,
  'F': PAL.girlDark
};

function getBoySprite(step = false) {
  const leg1 = step ? 'FF' : '..';
  const leg2 = step ? '..' : 'FF';
  return renderMatrix([
    '......CCCC........',
    '.....CCCCCC.......',
    '....CCCCCCCC......',
    '...RRRRRRRRRR.....',
    '..RRRRRRRRRRRR....',
    '..RRWBRRRRWBRR....',
    '..RRBBRRRRBBRR....',
    '..RRRRRRRRRRRR....',
    '..RRRRRRRRRRRR....',
    '...DDDDDDDDDD.....',
    '....' + leg1 + '....' + leg2 + '......'
  ], boyPalette, 3);
}

function getGirlSprite(step = false) {
  const leg1 = step ? 'FF' : '..';
  const leg2 = step ? '..' : 'FF';
  return renderMatrix([
    '.......YYOYY......',
    '.......YYOYY......',
    '....PPPPPPPPPP....',
    '...PPPPPPPPPPPP...',
    '..PPPPPPPPPPPPPP..',
    '..PPWRPPPPPPWRPP..',
    '..PPRRPPPPPPRRPP..',
    '..PPPPPPPPPPPPPP..',
    '..PPPPPPPPPPPPPP..',
    '...MMMMMMMMMMMM...',
    '....' + leg1 + '....' + leg2 + '......'
  ], girlPalette, 3);
}

function getHuggingSprite() {
  const hugPal = {
    '.': 'transparent',
    'C': PAL.boyCap,
    'R': PAL.boyBody,
    'D': PAL.boyDark,
    'Y': PAL.girlBow,
    'O': PAL.girlBowCenter,
    'P': PAL.girlBody,
    'M': PAL.girlDark,
    '^': PAL.eyeBlack,
    'B': PAL.blush,
    'L': '#ffb4c8',
    'F': '#781428'
  };
  return renderMatrix([
    '....CCCC...........YYOYY....',
    '...CCCCCC..........YYOYY....',
    '..CCCCCCCC.......PPPPPPPP...',
    '..RRRRRRRRRR...PPPPPPPPPP...',
    '.RRRRRRRRRRRR.PPPPPPPPPPPP..',
    '.RR^^RRRRLRPPPPPPP^^PPPPPP..',
    '.RRRRRRRRLRPPPPPPBLPPPPPPP..',
    '.RRRRRRRRRRRR.PPPPPPPPPPPP..',
    '..DDDDDDDDDD...MMMMMMMMMM...',
    '....FF..FF.......FF..FF.....'
  ], hugPal, 3);
}

function getHeartSprite(shiny = false, scale = 2) {
  const heartPal = {
    '.': 'transparent',
    'R': PAL.heart,
    'S': shiny ? PAL.heartShine : PAL.heart
  };
  return renderMatrix([
    '..RR..RR..',
    '.RSRRRRRR.',
    'RRSRRRRRRR',
    'RRRRRRRRRR',
    '.RRRRRRRR.',
    '..RRRRRR..',
    '...RRRR...',
    '....RR....'
  ], heartPal, scale);
}

function getFlowerSprite(withered = false) {
  const flPal = {
    '.': 'transparent',
    'Y': withered ? PAL.witheredPetal : PAL.flowerPetal,
    'C': withered ? PAL.witheredCenter : PAL.flowerCenter,
    'S': withered ? PAL.witheredStem : PAL.flowerStem
  };
  if (!withered) {
    return renderMatrix([
      '...YY...',
      '.YYYYYY.',
      '.YYCCYY.',
      '.YYYYYY.',
      '...YY...',
      '...SS...',
      '...SS...',
      '.SSSS...',
      '...SS...'
    ], flPal, 2);
  } else {
    return renderMatrix([
      '........',
      '....YY..',
      '...YYC..',
      '....YY..',
      '...S....',
      '...S....',
      '..S.....',
      '.SS.....',
      '..S.....'
    ], flPal, 2);
  }
}

function getMoonSprite() {
  const moonPal = {
    '.': 'transparent',
    'M': PAL.moon,
    'C': PAL.moonCrater
  };
  return renderMatrix([
    '.....MMMMMM.....',
    '...MMMMMMMMMM...',
    '..MMCCMMMMMMMM..',
    '.MMMCCMMMMCCMMM.',
    '.MMMMMMMMMCCMMM.',
    'MMMMMMMMMMMMMMMM',
    'MMCCMMMMMMMMMMMM',
    'MMCCMMMMMMCCMMMM',
    'MMMMMMMMMMCCMMMM',
    'MMMMMMMMMMMMMMMM',
    '.MMMMCCMMMMMMMM.',
    '.MMMMCCMMMMMMMM.',
    '..MMMMMMMMMMMM..',
    '...MMMMMMMMMM...',
    '.....MMMMMM.....'
  ], moonPal, 2);
}

// Pre-render static sprites
const sprBoyNormal = getBoySprite(false);
const sprBoyStep = getBoySprite(true);
const sprGirlNormal = getGirlSprite(false);
const sprGirlStep = getGirlSprite(true);
const sprHugging = getHuggingSprite();
const sprHeart = getHeartSprite(false, 2);
const sprHeartShiny = getHeartSprite(true, 2);
const sprFlower = getFlowerSprite(false);
const sprFlowerWithered = getFlowerSprite(true);
const sprMoon = getMoonSprite();

// --- STARS & FLOATING HEARTS PARTICLES ---
const stars = [];
for (let i = 0; i < 45; i++) {
  stars.push({
    xRatio: Math.random(),
    yRatio: Math.random() * 0.85,
    phase: Math.random() * Math.PI * 2,
    size: Math.random() < 0.2 ? 2.5 : 1.5,
    speed: 1.5 + Math.random() * 3.0
  });
}

const floatingHearts = [];

// --- CODE LINES BUILDER ---
function getCodeLines(crush) {
  return [
    {
      num: 1, tokens: [
        { t: 'async function ', c: PAL.codeKeyword },
        { t: 'goiConTim', c: PAL.codeFunction },
        { t: '(', c: PAL.codePunct },
        { t: crush, c: PAL.codeVar },
        { t: ') {', c: PAL.codePunct }
      ]
    },
    {
      num: 2, tokens: [
        { t: '  const ', c: PAL.codeKeyword },
        { t: 'chanThanh', c: PAL.codeVar },
        { t: ' = ', c: PAL.codePunct },
        { t: 'gather', c: PAL.codeFunction },
        { t: '(', c: PAL.codePunct },
        { t: '20', c: PAL.codeNumber },
        { t: ');', c: PAL.codePunct }
      ]
    },
    {
      num: 3, tokens: [
        { t: '  const ', c: PAL.codeKeyword },
        { t: 'moi', c: PAL.codeVar },
        { t: ' = ', c: PAL.codePunct },
        { t: 'await ', c: PAL.codeKeyword },
        { t: 'doi', c: PAL.codeFunction },
        { t: '(', c: PAL.codePunct },
        { t: 'chanThanh', c: PAL.codeVar },
        { t: ');', c: PAL.codePunct }
      ]
    },
    {
      num: 4, tokens: [
        { t: '  if ', c: PAL.codeKeyword },
        { t: '(!', c: PAL.codePunct },
        { t: crush, c: PAL.codeVar },
        { t: '.', c: PAL.codePunct },
        { t: 'dongY', c: PAL.codeVar },
        { t: ') ', c: PAL.codePunct },
        { t: 'return ', c: PAL.codeKeyword },
        { t: 'cho', c: PAL.codeFunction },
        { t: '();', c: PAL.codePunct }
      ]
    },
    { num: 5, tokens: [] },
    {
      num: 6, tokens: [
        { t: '  const ', c: PAL.codeKeyword },
        { t: 'hoa', c: PAL.codeVar },
        { t: ' = ', c: PAL.codePunct },
        { t: 'await ', c: PAL.codeKeyword },
        { t: crush, c: PAL.codeVar },
        { t: '.', c: PAL.codePunct },
        { t: 'trao', c: PAL.codeFunction },
        { t: '("', c: PAL.codePunct },
        { t: 'hoa', c: PAL.codeString },
        { t: '");', c: PAL.codePunct }
      ]
    },
    {
      num: 7, tokens: [
        { t: '  const ', c: PAL.codeKeyword },
        { t: 'qua', c: PAL.codeVar },
        { t: ' = ', c: PAL.codePunct },
        { t: 'goi', c: PAL.codeFunction },
        { t: '(', c: PAL.codePunct },
        { t: 'conTim', c: PAL.codeVar },
        { t: ', ', c: PAL.codePunct },
        { t: 'hoa', c: PAL.codeVar },
        { t: ');', c: PAL.codePunct }
      ]
    },
    { num: 8, tokens: [] },
    {
      num: 9, tokens: [
        { t: '  while ', c: PAL.codeKeyword },
        { t: '(', c: PAL.codePunct },
        { t: 'hoa', c: PAL.codeVar },
        { t: '.', c: PAL.codePunct },
        { t: 'tanUa', c: PAL.codeVar },
        { t: ') {', c: PAL.codePunct }
      ]
    },
    {
      num: 10, tokens: [
        { t: '    qua', c: PAL.codeVar },
        { t: '.', c: PAL.codePunct },
        { t: 'nhipDap', c: PAL.codeVar },
        { t: ' += ', c: PAL.codePunct },
        { t: 'tick', c: PAL.codeFunction },
        { t: '();', c: PAL.codePunct }
      ]
    },
    {
      num: 11, tokens: [
        { t: '  }', c: PAL.codePunct }
      ]
    },
    { num: 12, tokens: [] },
    {
      num: 13, tokens: [
        { t: '  await ', c: PAL.codeKeyword },
        { t: crush, c: PAL.codeVar },
        { t: '.', c: PAL.codePunct },
        { t: 'veDay', c: PAL.codeFunction },
        { t: '();', c: PAL.codePunct }
      ]
    },
    {
      num: 14, tokens: [
        { t: '  const ', c: PAL.codeKeyword },
        { t: 'tay', c: PAL.codeVar },
        { t: ' = ', c: PAL.codePunct },
        { t: 'giu', c: PAL.codeFunction },
        { t: '(', c: PAL.codePunct },
        { t: crush, c: PAL.codeVar },
        { t: ', ', c: PAL.codePunct },
        { t: 'chatHon', c: PAL.codeVar },
        { t: ');', c: PAL.codePunct }
      ]
    },
    {
      num: 15, tokens: [
        { t: '  if ', c: PAL.codeKeyword },
        { t: '(', c: PAL.codePunct },
        { t: 'cachRoi', c: PAL.codeVar },
        { t: ') ', c: PAL.codePunct },
        { t: 'return ', c: PAL.codeKeyword },
        { t: 'khong', c: PAL.codeFunction },
        { t: '(', c: PAL.codePunct },
        { t: 'tay', c: PAL.codeVar },
        { t: ');', c: PAL.codePunct }
      ]
    },
    {
      num: 16, tokens: [
        { t: '  return ', c: PAL.codeKeyword },
        { t: 'nangNiu', c: PAL.codeFunction },
        { t: '(', c: PAL.codePunct },
        { t: crush, c: PAL.codeVar },
        { t: ', ', c: PAL.codePunct },
        { t: 'suotDoi', c: PAL.codeVar },
        { t: ');', c: PAL.codePunct }
      ]
    },
    {
      num: 17, tokens: [
        { t: '}', c: PAL.codePunct }
      ]
    }
  ];
}

let codeLines = getCodeLines(customCrushName);

// --- TIMELINE CONTROLLER ---
function getTimelineState(sec) {
  const s = {
    activeLine: 2,
    boyPose: 'idle',
    girlPose: 'idle',
    boySpeech: null,
    girlSpeech: null,
    showHearts: false,
    bigShower: false,
    lyrics: '',
    walkProgress: 0
  };

  if (sec < 2.2) {
    s.activeLine = 2;
    s.boySpeech = customBoySpeech;
    s.lyrics = 'Gom chân thành đôi mươi...';
  } else if (sec < 6.5) {
    s.activeLine = 3;
    s.boySpeech = 'Huyền cười đẹp lắm';
    s.lyrics = '...để đổi lấy đôi môi em cười...';
  } else if (sec < 13.8) {
    s.activeLine = 4;
    s.boySpeech = 'Huyền đồng ý nhé?';
    s.lyrics = 'Chỉ cần em... đồng ý...';
  } else if (sec < 17.8) {
    s.activeLine = 6;
    s.girlPose = 'holdingFlower';
    s.lyrics = 'Em trao nhành hoa...';
  } else if (sec < 21.0) {
    s.activeLine = 7;
    s.boyPose = 'holdingHeart';
    s.girlPose = 'holdingFlower';
    s.boySpeech = 'Hiếu gói tim tặng Huyền';
    s.lyrics = '...Hiếu gói con tim làm quà...';
  } else if (sec < 23.2) {
    s.activeLine = 9;
    s.girlPose = 'flowerWithering';
    s.boySpeech = 'sợ hoa tàn mất...';
    s.lyrics = '...nhưng sợ hoa sẽ tàn úa...';
  } else if (sec < 28.0) {
    s.activeLine = 10;
    s.showHearts = true;
    s.lyrics = '...theo nhịp đập thời gian...';
  } else if (sec < 31.0) {
    s.activeLine = 13;
    s.boyPose = 'walking';
    s.girlPose = 'walking';
    s.boySpeech = 'về đây với Hiếu';
    s.lyrics = 'Xin Huyền về đây với Hiếu...';
    s.walkProgress = Math.min(1, Math.max(0, (sec - 28.0) / 3.0));
  } else if (sec < 36.2) {
    s.activeLine = 14;
    s.boyPose = 'hugging';
    s.girlPose = 'hugging';
    s.showHearts = true;
    s.walkProgress = 1;
    s.lyrics = '...Hiếu giữ Huyền bên trong vòng tay...';
  } else if (sec < 39.8) {
    s.activeLine = 15;
    s.boyPose = 'hugging';
    s.girlPose = 'hugging';
    s.showHearts = true;
    s.walkProgress = 1;
    s.boySpeech = 'ở bên Hiếu mãi nhé?';
    s.lyrics = '...đâu sợ mai cách rời...';
  } else {
    s.activeLine = 16;
    s.boyPose = 'hugging';
    s.girlPose = 'hugging';
    s.showHearts = true;
    s.bigShower = true;
    s.walkProgress = 1;
    s.girlSpeech = customGirlSpeech;
    s.lyrics = '...Có Huyền rồi, Hiếu nâng niu suốt đời...';
  }

  return s;
}

// --- MARQUEE TEXT ---
let marqueeX = 0;
const marqueeStr = 'Gói Con Tim Làm Quà • Tặng Huyền nè ❤️ • Phạm Minh Hiếu • karaoke mode • ';

// --- RESIZE CANVAS FOR HIGH DPI ---
let logicalW = 400;
let logicalH = 740;

function resizeCanvas() {
  const rect = canvas.getBoundingClientRect();
  const dpr = window.devicePixelRatio || 1;
  logicalW = rect.width;
  logicalH = rect.height;

  canvas.width = Math.floor(logicalW * dpr);
  canvas.height = Math.floor(logicalH * dpr);

  ctx.setTransform(dpr, 0, 0, dpr, 0, 0);
  ctx.imageSmoothingEnabled = false;
}

window.addEventListener('resize', resizeCanvas);

// --- MAIN RENDER LOOP ---
let lastFrameTime = performance.now();

function renderLoop(now) {
  const delta = (now - lastFrameTime) / 1000;
  lastFrameTime = now;

  // Sync time
  if (isPlaying) {
    if (isAudioActive && !audio.paused) {
      currentTime = audio.currentTime;
      if (currentTime >= DURATION) {
        audio.currentTime = 0;
        currentTime = 0;
      }
    } else {
      currentTime += delta;
      if (currentTime >= DURATION) {
        currentTime = 0;
      }
    }
    updateControlsUI();
  }

  // Update marquee
  marqueeX -= delta * 45;
  if (marqueeX < -450) marqueeX = 0;

  // Update floating particles
  const state = getTimelineState(currentTime);
  if (state.showHearts && Math.random() < (state.bigShower ? 0.35 : 0.12)) {
    floatingHearts.push({
      x: logicalW * 0.5 + (Math.random() - 0.5) * 80,
      y: logicalH * 0.38,
      speedY: 40 + Math.random() * 50,
      drift: Math.random() * Math.PI * 2,
      scale: 0.8 + Math.random() * 0.8,
      life: 1.0
    });
  }

  for (let i = floatingHearts.length - 1; i >= 0; i--) {
    const h = floatingHearts[i];
    h.y -= h.speedY * delta;
    h.drift += delta * 3.0;
    h.x += Math.sin(h.drift) * 20.0 * delta;
    h.life -= delta * 0.55;
    if (h.life <= 0 || h.y < 20) {
      floatingHearts.splice(i, 1);
    }
  }

  // Render Scene
  drawScene(state);

  requestAnimationFrame(renderLoop);
}

// --- DRAWING FUNCTIONS ---
function drawScene(state) {
  ctx.clearRect(0, 0, logicalW, logicalH);

  const sceneH = Math.floor(logicalH * 0.40);
  const marqueeH = 26;
  const codeTop = sceneH + marqueeH;
  const codeH = logicalH - codeTop;

  // 1. Top Scene
  drawTopScene(sceneH, state);

  // 2. Marquee Divider
  drawMarquee(sceneH, marqueeH);

  // 3. Code Panel
  drawCodePanel(codeTop, codeH, state);
}

function drawTopScene(h, state) {
  // Night sky gradient
  const grad = ctx.createLinearGradient(0, 0, 0, h);
  grad.addColorStop(0, PAL.skyTop);
  grad.addColorStop(1, PAL.skyBottom);
  ctx.fillStyle = grad;
  ctx.fillRect(0, 0, logicalW, h);

  // Twinkling stars
  for (let i = 0; i < stars.length; i++) {
    const s = stars[i];
    const sx = s.xRatio * logicalW;
    const sy = s.yRatio * (h - 30);
    const alpha = 0.3 + 0.7 * (Math.sin(currentTime * s.speed + s.phase) * 0.5 + 0.5);

    ctx.fillStyle = `rgba(255, 255, 255, ${alpha})`;
    ctx.fillRect(Math.floor(sx), Math.floor(sy), s.size, s.size);
  }

  // Pixel Moon
  const moonX = logicalW - sprMoon.width - 24;
  const moonY = 16;
  ctx.fillStyle = 'rgba(255, 255, 255, 0.08)';
  ctx.beginPath();
  ctx.arc(moonX + sprMoon.width / 2, moonY + sprMoon.height / 2, sprMoon.width / 2 + 8, 0, Math.PI * 2);
  ctx.fill();
  ctx.drawImage(sprMoon, moonX, moonY);

  // Ground & Grass
  const groundY = h - 20;
  ctx.fillStyle = PAL.ground;
  ctx.fillRect(0, groundY, logicalW, 20);

  ctx.strokeStyle = PAL.grass;
  ctx.lineWidth = 2;
  ctx.beginPath();
  ctx.moveTo(0, groundY);
  ctx.lineTo(logicalW, groundY);
  ctx.stroke();

  // Little grass tufts
  ctx.fillStyle = PAL.grass;
  for (let gx = 18; gx < logicalW; gx += 40) {
    ctx.fillRect(gx, groundY - 4, 2, 4);
    ctx.fillRect(gx + 3, groundY - 6, 2, 6);
  }

  // Characters positioning
  const boyStartX = logicalW * 0.28;
  const girlStartX = logicalW * 0.72;
  const meetX = logicalW * 0.50;

  const boyX = boyStartX + (meetX - 22 - boyStartX) * state.walkProgress;
  const girlX = girlStartX + (meetX + 22 - girlStartX) * state.walkProgress;
  const charY = groundY - 44;

  const step = Math.floor(currentTime * 6) % 2 === 0;

  if (state.boyPose === 'hugging') {
    // Hugging pose
    const hugX = meetX - sprHugging.width / 2;
    ctx.drawImage(sprHugging, hugX, charY + 4);

    if (state.boySpeech) {
      drawSpeechBubble(state.boySpeech, hugX + 16, charY - 22, true);
    }
    if (state.girlSpeech) {
      drawSpeechBubble(state.girlSpeech, hugX + sprHugging.width - 16, charY - 22, false);
    }
  } else {
    // Boy
    const bSpr = state.boyPose === 'walking' ? (step ? sprBoyStep : sprBoyNormal) : sprBoyNormal;
    ctx.drawImage(bSpr, boyX - bSpr.width / 2, charY);

    if (state.boyPose === 'holdingHeart') {
      const shiny = Math.floor(currentTime * 8) % 2 === 0;
      ctx.drawImage(shiny ? sprHeartShiny : sprHeart, boyX + 14, charY - 8);
    }
    if (state.boySpeech) {
      drawSpeechBubble(state.boySpeech, boyX, charY - 20, true);
    }

    // Girl
    const gSpr = state.girlPose === 'walking' ? (step ? sprGirlStep : sprGirlNormal) : sprGirlNormal;
    ctx.drawImage(gSpr, girlX - gSpr.width / 2, charY);

    if (state.girlPose === 'holdingFlower') {
      ctx.drawImage(sprFlower, girlX - 20, charY - 8);
    } else if (state.girlPose === 'flowerWithering') {
      ctx.drawImage(sprFlowerWithered, girlX - 20, charY - 8);
    }

    if (state.girlSpeech) {
      drawSpeechBubble(state.girlSpeech, girlX, charY - 20, false);
    }
  }

  // Floating Hearts
  for (let i = 0; i < floatingHearts.length; i++) {
    const fh = floatingHearts[i];
    ctx.globalAlpha = Math.max(0, Math.min(1, fh.life));
    ctx.drawImage(sprHeart, fh.x, fh.y);
  }
  ctx.globalAlpha = 1.0;

}

function drawSpeechBubble(text, targetX, targetY, pointLeft) {
  ctx.font = 'bold 12px "Plus Jakarta Sans", sans-serif';
  const padX = 10;
  const padY = 6;
  const metrics = ctx.measureText(text);
  const w = metrics.width + padX * 2;
  const h = 24;

  let bx = targetX - w / 2;
  let by = targetY - h;

  if (bx < 8) bx = 8;
  if (bx + w > logicalW - 8) bx = logicalW - 8 - w;

  // Rounded bubble
  ctx.fillStyle = 'rgba(255, 255, 255, 0.95)';
  ctx.strokeStyle = 'rgba(200, 210, 225, 0.8)';
  ctx.lineWidth = 1;

  ctx.beginPath();
  roundRect(ctx, bx, by, w, h, 6);
  ctx.fill();
  ctx.stroke();

  // Tail
  ctx.fillStyle = 'rgba(255, 255, 255, 0.95)';
  ctx.beginPath();
  ctx.moveTo(targetX - 4, by + h);
  ctx.lineTo(targetX + 4, by + h);
  ctx.lineTo(targetX, by + h + 5);
  ctx.closePath();
  ctx.fill();

  // Text
  ctx.fillStyle = '#1e1e24';
  ctx.fillText(text, bx + padX, by + 16);
}


function drawMarquee(y, h) {
  ctx.fillStyle = '#12151d';
  ctx.fillRect(0, y, logicalW, h);

  ctx.strokeStyle = 'rgba(255, 255, 255, 0.08)';
  ctx.lineWidth = 1;
  ctx.beginPath();
  ctx.moveTo(0, y);
  ctx.lineTo(logicalW, y);
  ctx.moveTo(0, y + h);
  ctx.lineTo(logicalW, y + h);
  ctx.stroke();

  // Text inside clip
  ctx.save();
  ctx.beginPath();
  ctx.rect(0, y, logicalW, h);
  ctx.clip();

  ctx.fillStyle = '#cbd5e1';
  ctx.font = '11px "Plus Jakarta Sans", sans-serif';
  const full = marqueeStr + marqueeStr + marqueeStr;
  ctx.fillText(full, marqueeX, y + 17);
  ctx.restore();
}

function drawCodePanel(y, h, state) {
  ctx.fillStyle = '#1e1e1e';
  ctx.fillRect(0, y, logicalW, h);

  const lineH = 22;
  const startY = y + 14;
  const gutterW = 34;

  ctx.font = '13px "Fira Code", monospace';

  for (let i = 0; i < codeLines.length; i++) {
    const line = codeLines[i];
    const curY = startY + i * lineH;
    if (curY + lineH > y + h - 35) break;

    const isActive = line.num === state.activeLine;

    // Highlight background
    if (isActive) {
      ctx.fillStyle = PAL.codeLineHl;
      ctx.fillRect(0, curY - 2, logicalW, lineH);

      // Debugger Arrow ▶
      ctx.fillStyle = PAL.codeArrow;
      ctx.beginPath();
      ctx.moveTo(6, curY + 2);
      ctx.lineTo(14, curY + lineH / 2 - 2);
      ctx.lineTo(6, curY + lineH - 6);
      ctx.closePath();
      ctx.fill();
    }

    // Line number in gutter
    ctx.fillStyle = PAL.codeGutter;
    ctx.fillText(line.num.toString(), 18, curY + 14);

    // Tokens
    let tokenX = gutterW + 12;
    for (let t = 0; t < line.tokens.length; t++) {
      const tok = line.tokens[t];
      ctx.fillStyle = tok.c;
      ctx.fillText(tok.t, tokenX, curY + 14);
      tokenX += ctx.measureText(tok.t).width;
    }
  }

  // Karaoke Subtitle Bar
  if (isKaraokeEnabled && state.lyrics) {
    const subH = 32;
    const subY = y + h - subH - 12;

    ctx.font = 'bold 13px "Plus Jakarta Sans", sans-serif';
    const lMetrics = ctx.measureText(state.lyrics);
    const boxW = lMetrics.width + 28;
    const boxX = (logicalW - boxW) / 2;

    ctx.fillStyle = 'rgba(18, 20, 28, 0.9)';
    ctx.strokeStyle = PAL.heart;
    ctx.lineWidth = 1;

    ctx.beginPath();
    roundRect(ctx, boxX, subY, boxW, subH, 8);
    ctx.fill();
    ctx.stroke();

    ctx.fillStyle = '#ffe4ec';
    ctx.fillText(state.lyrics, boxX + 14, subY + 20);
  }
}

function roundRect(context, x, y, width, height, radius) {
  context.beginPath();
  context.moveTo(x + radius, y);
  context.lineTo(x + width - radius, y);
  context.quadraticCurveTo(x + width, y, x + width, y + radius);
  context.lineTo(x + width, y + height - radius);
  context.quadraticCurveTo(x + width, y + height, x + width - radius, y + height);
  context.lineTo(x + radius, y + height);
  context.quadraticCurveTo(x, y + height, x, y + height - radius);
  context.lineTo(x, y + radius);
  context.quadraticCurveTo(x, y, x + radius, y);
  context.closePath();
}

// --- CONTROLS & INTERACTION ---
function updateControlsUI() {
  const min = Math.floor(currentTime / 60);
  const sec = Math.floor(currentTime % 60);
  const totMin = Math.floor(DURATION / 60);
  const totSec = Math.floor(DURATION % 60);

  timeDisplay.textContent = `${String(min).padStart(2, '0')}:${String(sec).padStart(2, '0')} / ${String(totMin).padStart(2, '0')}:${String(totSec).padStart(2, '0')}`;

  const pct = Math.min(100, Math.max(0, (currentTime / DURATION) * 100));
  scrubFill.style.width = `${pct}%`;
  scrubHandle.style.left = `${pct}%`;
}

function togglePlay() {
  isPlaying = !isPlaying;

  if (isPlaying) {
    iconPlay.classList.add('hidden');
    iconPause.classList.remove('hidden');
    playOverlay.classList.add('hidden');

    audio.play().then(() => {
      isAudioActive = true;
    }).catch(() => {
      // Audio autoplay restrictions or file issue
      isAudioActive = false;
    });
  } else {
    iconPlay.classList.remove('hidden');
    iconPause.classList.add('hidden');
    audio.pause();
  }
}

function restart() {
  currentTime = 0;
  audio.currentTime = 0;
  if (!isPlaying) {
    togglePlay();
  } else {
    updateControlsUI();
  }
}

function seekTo(sec) {
  currentTime = Math.max(0, Math.min(DURATION, sec));
  audio.currentTime = currentTime;
  updateControlsUI();
}

// Event Listeners
bigPlayBtn.addEventListener('click', () => {
  togglePlay();
});

btnPlayPause.addEventListener('click', togglePlay);
btnRestart.addEventListener('click', restart);

btnKaraokeToggle.addEventListener('click', () => {
  isKaraokeEnabled = !isKaraokeEnabled;
  btnKaraokeToggle.classList.toggle('active', isKaraokeEnabled);
});

btnMute.addEventListener('click', () => {
  isMuted = !isMuted;
  audio.muted = isMuted;
  btnMute.style.opacity = isMuted ? '0.5' : '1';
});

// Scrubbing
let isDraggingScrub = false;

function handleScrub(e) {
  const rect = scrubContainer.getBoundingClientRect();
  const clickX = e.clientX - rect.left;
  const ratio = Math.max(0, Math.min(1, clickX / rect.width));
  seekTo(ratio * DURATION);
}

scrubContainer.addEventListener('mousedown', (e) => {
  isDraggingScrub = true;
  handleScrub(e);
});

window.addEventListener('mousemove', (e) => {
  if (isDraggingScrub) {
    handleScrub(e);
  }
});

window.addEventListener('mouseup', () => {
  isDraggingScrub = false;
});

// Toggle Phone Frame Mode vs Expanded
btnTogglePhone.addEventListener('click', () => {
  playerWrapper.classList.toggle('mode-phone');
  playerWrapper.classList.toggle('mode-expanded');
  const isPhone = playerWrapper.classList.contains('mode-phone');
  document.querySelector('.btn-label-mode').textContent = isPhone ? 'Khung Phone' : 'Mở Rộng';
  setTimeout(resizeCanvas, 100);
});

// Fullscreen
btnFullscreen.addEventListener('click', () => {
  if (!document.fullscreenElement) {
    playerWrapper.requestFullscreen().catch(() => { });
  } else {
    document.exitFullscreen().catch(() => { });
  }
});

// --- CUSTOMIZE MODAL ---
btnCustomize.addEventListener('click', () => {
  inputCrush.value = customCrushName;
  inputSpeech1.value = customBoySpeech;
  inputSpeech2.value = customGirlSpeech;
  modalBackdrop.classList.remove('hidden');
});

btnCloseModal.addEventListener('click', () => {
  modalBackdrop.classList.add('hidden');
});

modalBackdrop.addEventListener('click', (e) => {
  if (e.target === modalBackdrop) {
    modalBackdrop.classList.add('hidden');
  }
});

btnResetDefault.addEventListener('click', () => {
  inputCrush.value = 'Huyền';
  inputSpeech1.value = 'Hiếu thích Huyền';
  inputSpeech2.value = 'ừ, Huyền đồng ý';
});

btnSaveCustomize.addEventListener('click', () => {
  customCrushName = inputCrush.value.trim() || 'Huyền';
  customBoySpeech = inputSpeech1.value.trim() || 'Hiếu thích Huyền';
  customGirlSpeech = inputSpeech2.value.trim() || 'ừ, Huyền đồng ý';

  localStorage.setItem('crushName', customCrushName);
  localStorage.setItem('boySpeech', customBoySpeech);
  localStorage.setItem('girlSpeech', customGirlSpeech);

  codeLines = getCodeLines(customCrushName);
  modalBackdrop.classList.add('hidden');
});

// --- KEYBOARD SHORTCUTS ---
window.addEventListener('keydown', (e) => {
  // If modal is open, don't trigger global shortcuts
  if (!modalBackdrop.classList.contains('hidden')) return;

  if (e.code === 'Space') {
    e.preventDefault();
    togglePlay();
  } else if (e.code === 'KeyR') {
    e.preventDefault();
    restart();
  } else if (e.code === 'KeyT') {
    e.preventDefault();
    isKaraokeEnabled = !isKaraokeEnabled;
    btnKaraokeToggle.classList.toggle('active', isKaraokeEnabled);
  } else if (e.code === 'KeyC') {
    e.preventDefault();
    btnCustomize.click();
  } else if (e.code === 'KeyF') {
    e.preventDefault();
    btnFullscreen.click();
  } else if (e.code === 'ArrowLeft') {
    e.preventDefault();
    seekTo(currentTime - 3);
  } else if (e.code === 'ArrowRight') {
    e.preventDefault();
    seekTo(currentTime + 3);
  }
});

// Init
resizeCanvas();
updateControlsUI();
requestAnimationFrame(renderLoop);
