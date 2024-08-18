(function ($) {
  "use strict";

  // Collapse the navbar on link click
  $('.navbar-collapse a').click(function () {
    $(".navbar-collapse").collapse('hide');
  });

  // Smooth scrolling for internal links
  $('.smoothscroll').click(function (event) {
    event.preventDefault();
    let el = $(this).attr('href');
    let elWrapped = $(el);

    if (elWrapped.length) { // Check if the element exists
      let header_height = $('.navbar').height();
      scrollToDiv(elWrapped, header_height);
    } else {
      console.error('Element not found:', el); // Log error if element doesn't exist
    }

    return false;
  });

  // Function to scroll to a specific div
  function scrollToDiv(element, navheight) {
    let offset = element.offset();
    let offsetTop = offset.top;
    let totalScroll = offsetTop - navheight;

    $('body,html').animate({
      scrollTop: totalScroll
    }, 300);
  }
})(jQuery);
