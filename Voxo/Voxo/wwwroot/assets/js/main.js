
$(document).on("click", ".modal-btn", function (e) {
    e.preventDefault();

    let url = $(this).attr("href");

    fetch(url)
        .then(response => {

            console.log(response)
            if (response.ok) {
                return response.text()
            }
            else {
                alert("Bilinmedik bir xeta bas verdi!")
            }
        })
        .then(data => {
            $("#quick-view .modal-body").html(data)
            $("#quick-view").modal('show')
        })
})

$(document).on("click", ".modal-close", function (e) {
    e.preventDefault();
    $(".modal-body-html").remove()
})

$(document).on("click", ".addtocart-btn", function (e) {
    e.preventDefault();

    let url = $(this).attr("href");

    fetch(url)
        .then(response => {

            console.log(response)
            if (response.ok) {
                return response.text()
            }
            else {
                alert("Bilinmedik bir xeta bas verdi!")
            }
        })
        .then(data => {
            $(".cart-partial .onhover-div").html(data)
            $(".show-total").text($(".total-price").text())
            
        })
})

$(document).on("click", "#edit-profile-btn", function (e) {
    e.preventDefault();

    let url = $(this).attr("href");

    fetch(url)
        .then(response => {

            console.log(response)
            if (response.ok) {
                return response.text()
            }
            else {
                alert("Something went wrong!")
            }
        })
        .then(data => {
            $(".edit-profile-modal").html(data)
        })
})
$(document).on("click", "#change-password-btn", function (e) {
    e.preventDefault();

    let url = $(this).attr("href");

    fetch(url)
        .then(response => {

            console.log(response)
            if (response.ok) {
                return response.text()
            }
            else {
                alert("Something went wrong!")
            }
        })
        .then(data => {
            $(".changepassword-modal").html(data)
        })
})




