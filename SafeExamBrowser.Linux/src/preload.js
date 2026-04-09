const { contextBridge, ipcRenderer } = require('electron');

contextBridge.exposeInMainWorld('seb', {
    version: '4.0.0',
    platform: 'linux',
    // Future bridge to .NET logic can go here
});
