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
    let header_height = $('.navbar').height();

    scrollToDiv(elWrapped, header_height);
    return false;

    function scrollToDiv(element, navheight) {
      let offset = element.offset();
      let offsetTop = offset.top;
      let totalScroll = offsetTop - navheight;

      $('body,html').animate({
        scrollTop: totalScroll
      }, 300);
    }
  });

})(jQuery);
$(document).ready(function () {
  // Fetch book data from your API and display them as cards
  fetch('http://localhost:5106/api/Book')
      .then(response => {
        if (!response.ok) {
          throw new Error('Failed to fetch books.');
        }
        return response.json();
      })
      .then(data => {
        // Loop through the book data and generate product cards
        data.forEach(book => {
          const cardHtml = `
          <div class="col-md-4">
            <div class="card" style="width: 18rem;">
              <img src="${book.imageUrl}" class="card-img-top" alt="${book.title}">
              <div class="card-body">
                <h5 class="card-title">${book.title}</h5>
                <h6 class="card-subtitle mb-2 text-muted">${book.author}</h6>
                <h6 class="card-subtitle mb-2 text-muted">${book.publicationYear}</h6>
                <p class="card-text">${book.price.toFixed(2)} USD</p>
              </div>
            </div>
          </div>`;
          // Append the generated card HTML to the product cards container
          $('#product-cards-container').append(cardHtml);
        });
      })
      .catch(error => {
        console.error('Error fetching book data:', error);
        alert("Failed to load books: " + error.message);
      });
});
