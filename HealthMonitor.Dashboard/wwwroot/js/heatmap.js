window.initializeHeatmap = (heatmapData) => {
    
    const map = L.map('usHeatmap').setView([37.8, -96], 4); // Center over us map

    
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(map);

    const heat = L.heatLayer(heatmapData, {
        radius: 25,  
        blur: 15,    
        maxZoom: 10 
    }).addTo(map);
};
