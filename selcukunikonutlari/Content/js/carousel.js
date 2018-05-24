

function mycarousel_initCallback(carousel) {

    jQuery('#next').bind('click', function() {
        carousel.next();
        return false;
    });

    jQuery('#prev').bind('click', function() {
        carousel.prev();
        return false;
    });
    
    

        $('.carousel ul li').mouseenter(function(e) {

            $(this).children('a').children('span').fadeIn(200);
        }).mouseleave(function(e) {

            $(this).children('a').children('span').fadeOut(200);
        });
  
    
    
};

jQuery(document).ready(function() {
    jQuery(".carousel").jcarousel({
        scroll: 5,
        auto: 10,
        initCallback: mycarousel_initCallback,
        buttonNextHTML: null,
        buttonPrevHTML: null,
        wrap: 'last',
        animation: 800
    });
});