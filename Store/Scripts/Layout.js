

function GetCartItemQuantity() {
    url = "../Shopping/GetCartItemQuantity"
    fetch(url,
        {
            method: "POST",
            headers: {
                'Content-Type': 'application/json' 
            },
        }
    )
        .then(Response => {
            return Response.json();
        })
        .then(data => {
            if (data.ajaxStatus != 401) {
                if (data.CartItemQuantity != null) {
                    document.getElementById("cartItemsQuantity").textContent = data.CartItemQuantity;
                } else {
                    document.getElementById("cartItemsQuantity").textContent = 0;
                }
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
                'Content-Type': 'application/json'
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
