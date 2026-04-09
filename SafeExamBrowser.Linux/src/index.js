const { app, BrowserWindow, globalShortcut } = require('electron');
const path = require('path');

let mainWindow;

function createWindow() {
  mainWindow = new BrowserWindow({
    width: 1024,
    height: 768,
    fullscreen: true,
    kiosk: true, // Typical for SEB
    autoHideMenuBar: true,
    webPreferences: {
      nodeIntegration: false,
      contextIsolation: true,
      preload: path.join(__dirname, 'preload.js')
    }
  });

  // Load a default exam page or the specific URL from configuration
  mainWindow.loadFile('src/index.html');

  // Disable common "escape" keys to maintain lockdown
  globalShortcut.register('CommandOrControl+Shift+I', () => {
    return false; // Prevent devtools
  });

  mainWindow.on('closed', () => {
    mainWindow = null;
  });
}

// Security: Prevent multiple instances
if (!app.requestSingleInstanceLock()) {
  app.quit();
} else {
  app.on('ready', createWindow);
}

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit();
  }
});

app.on('activate', () => {
  if (mainWindow === null) {
    createWindow();
  }
});

// Security: Prevent navigation away from the specified domain (placeholder logic)
app.on('web-contents-created', (event, contents) => {
  contents.on('will-navigate', (event, navigationUrl) => {
    // Here we would implement SEB URL Filtering
    console.log(`Navigating to: ${navigationUrl}`);
  });
});
