grecaptcha.ready(function () {
    grecaptcha.execute('6LdoNBIqAAAAABPwyhXYJInO4cjAIh-I6l52_0PN').then(function (token) {
        document.getElementById('recaptcha').value = token;
    });
});