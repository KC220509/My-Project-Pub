
    var currentSlide = 0;
    var slides = document.querySelectorAll(".content_slider_bottom .slide");

    function showSlide(n) {
        if (n >= slides.length) {
            currentSlide = 0;
        } else if (n < 0) {
            currentSlide = slides.length - 1;
        }

        for (var i = 0; i < slides.length; i++) {
            slides[i].style.display = "none";
        }

        slides[currentSlide].style.display = "block";
        currentSlide++;
    }

    function autoSlide() {
        showSlide(currentSlide);
        setTimeout(autoSlide, 3000); // Tự động lướt sau 3 giây
    }

    autoSlide();

