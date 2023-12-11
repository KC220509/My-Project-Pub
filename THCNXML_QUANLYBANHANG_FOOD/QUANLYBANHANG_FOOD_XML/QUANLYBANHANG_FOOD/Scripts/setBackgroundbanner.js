const banner = document.getElementById('banner');
const images = ["url(../assets/banner.png)", "url(../assets/banner1.png)", "url(../assets/banner2.png)", "url(../assets/banner3.jpg)"];
let currentImage = 0;

function changeBackground() {
    currentImage = (currentImage + 1) % images.length;
    banner.style.backgroundImage = images[currentImage];
}

setInterval(changeBackground, 3000);
