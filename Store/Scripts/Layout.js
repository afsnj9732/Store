function GetCartItemQuantity() {
    url = "../Shopping/GetCartItemQuantity"
    fetch(url,
        {
            method: "POST",
            headers: {
                'Content-Type': 'application/json' ,
                'RequestVerificationToken': antiForgeryToken 
            },
        }
    )
        .then(Response => {
            return Response.json();
        })
        .then(data => {
            if (data.ajaxStatus!=401) {
                document.getElementById("cartItemsQuantity").textContent = data.CartItemQuantity;
            }

        })
}
function AddCart(listID) {
    var data = { productID: listID }
    url = "../Shopping/AddCart"
    fetch(url,
        {
            method: "POST",
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': antiForgeryToken 
            },
            body: JSON.stringify(data)
        }
    )
        .then(Response => {
            return Response.json();
        })
        .then(data => {
            if (data.ajaxStatus == 401) {
                window.location.href = '../Validate/Login'
            } else {
                GetCartItemQuantity();

            }
        })

}
document.addEventListener('DOMContentLoaded', function () {
    GetCartItemQuantity();
});
