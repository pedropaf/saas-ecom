// Stripe Card Details form

var stripeResponseHandler = function (status, response) {
    var $form = $('#card-form');

    var errorMessages = {
        incorrect_number: "The card number is incorrect.",
        invalid_number: "The card number is not a valid credit card number.",
        invalid_expiry_month: "The card's expiration month is invalid.",
        invalid_expiry_year: "The card's expiration year is invalid.",
        invalid_cvc: "The card's security code is invalid.",
        expired_card: "The card has expired.",
        incorrect_cvc: "The card's security code is incorrect.",
        incorrect_zip: "The card's zip code failed validation.",
        card_declined: "The card was declined.",
        missing: "There is no card on a customer that is being charged.",
        processing_error: "An error occurred while processing the card.",
        rate_limit: "An error occurred due to requests hitting the API too quickly. Please let us know if you're consistently running into this error."
    };

    if (response.error && response.error.type == 'card_error') {
        // Show the errors on the form
        $form.find('.payment-errors').text(errorMessages[response.error.code]);
        $form.find('button').prop('disabled', false);

        $form.find('#CreditCard_CardNumber').addClass('input-validation-error');
        $form.find('#CreditCard_ExpirationMonth').addClass('input-validation-error');
        $form.find('#CreditCard_ExpirationYear').addClass('input-validation-error');
        $form.find('#CreditCard_Cvc').addClass('input-validation-error');

    } else {
        // token contains id, last4, and card type
        var token = response.id;

        // Insert the token into the form so it gets submitted to the server
        $form.append($('<input type="hidden" name="CreditCard.StripeToken" />').val(token));

        if (typeof response.card !== 'undefined') {
            $form.append($('<input type="hidden" name="CreditCard.Type" />').val(response.card.brand));
            $form.append($('<input type="hidden" name="CreditCard.Last4" />').val(response.card.last4));
            $form.append($('<input type="hidden" name="CreditCard.StripeId" />').val(response.card.id));
            $form.append($('<input type="hidden" name="CreditCard.Fingerprint" />').val(response.card.fingerprint));
            $form.append($('<input type="hidden" name="CreditCard.CardNumber" />').val(response.card.fingerprint));
            $form.append($('<input type="hidden" name="CreditCard.CardCountry" />').val(response.card.country));
        }

        // Remove CardNumber field from form
        var $card = $form.find('#CreditCard_CardNumber');
        $card.removeAttr('name');
        $card.removeAttr('id');

        // and re-submit
        $form.get(0).submit();
    }
};

jQuery(function ($) {

    var stripeKey = $('#stripe-publishable-key').val();
    Stripe.setPublishableKey($('#stripe-publishable-key').val());

    $('#card-form').submit(function (e) {
        var $form = $(this);

        // Disable the submit button to prevent repeated clicks
        $form.find('button').prop('disabled', true);

        Stripe.card.createToken($form, stripeResponseHandler);

        // Prevent the form from submitting with the default action
        return false;
    });
});
