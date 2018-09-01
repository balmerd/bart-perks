(function ($) {
    Drupal.behaviors.bart_perk_custom = {
        attach: function (context) {
            //alert();
        }
    };
    $(document).ready(function ($) {
        if (Drupal.settings.bart_perk !== undefined) {
            var pt_dial = Drupal.settings.bart_perk.pt_dial;
            if (pt_dial == true) {
                // Get all the Meters
                const meters = document.querySelectorAll('svg[data-value] .meter');

                meters.forEach((path) => {
                    // Get the length of the path
                    let length = path.getTotalLength();
                    // console.log(length) once and hardcode the stroke-dashoffset and stroke-dasharray in the SVG if possible
                    // or uncomment to set it dynamically
                    // path.style.strokeDashoffset = length;
                    // path.style.strokeDasharray = length;

                    // Get the value of the meter
                    let value = parseInt(path.parentNode.getAttribute('data-value'));
                    // Calculate the percentage of the total length
                    let to = length * ((100 - value) / 100);
                    // Trigger Layout in Safari hack https://jakearchibald.com/2013/animated-line-drawing-svg/
                    path.getBoundingClientRect();
                    // Set the Offset
                    path.style.strokeDashoffset = Math.max(0, to);
                });
            }
        }
        var url = window.location.pathname;;
        var split_url = url.split('/');
        if (split_url !== '') {
            if ((split_url[3] == 'point-trip' || split_url[3] == 'offers') && split_url[4] == 'history') {
                if (split_url[3] == 'point-trip') {
                    var trip_history_class = '.pt-history-table';
                }
                if (split_url[3] == 'offers') {
                    var trip_history_class = '.offer-history-table';
                }
                $(".calender").hide();
                $(".filter-by-date").click(function () {
                    $(".calender").toggle();
                });
                $('.calender').pignoseCalendar({
                    multiple: true, theme: 'light', initialize: false,
                    select: function () {
                        var pathname = window.location.pathname;
                        var min_date = $('.pignose-calendar-unit-first-active').attr('data-date');
                        var max_date = $('.pignose-calendar-unit-second-active').attr('data-date');
                        if (min_date !== undefined && max_date !== undefined) {
                            $.ajax({
                                type: 'POST',
                                url: '/ajax/post/filter-data',
                                data: 'min_date=' + min_date + '&max_date=' + max_date + '&page_url=' + split_url[3],
                                success: function (data) {
                                    $(trip_history_class).html(data.filter_result);
                                },
                            });
                        }
                    },
                });
                $('.cus-perk-filter-form input').keyup(function () {
                    if ($('.cus-perk-filter-form .min-pt').val() && $('.cus-perk-filter-form .max-pt').val()) {
                        $('.cus-perk-filter-form .cus-enter-btn').removeClass('btn-disabled');
                    } else {
                        $('.cus-perk-filter-form .cus-enter-btn').addClass('btn-disabled');
                    }
                });
                $('.cus-perk-filter-form .cus-enter-btn span').click(function () {
                    if ($('.min-pt').val() && $('.max-pt').val()) {
                        // alert();
                        var min_pts = $('.min-pt').val();
                        var max_pts = $('.max-pt').val();

                        $.ajax({
                            type: 'POST',
                            url: '/ajax/post/filter-data',
                            data: 'min_points=' + min_pts + '&max_points=' + max_pts + '&page_url=' + split_url[3],
                            success: function (data) {
                                $(trip_history_class).html(data.filter_result);
                            },
                        });
                    }
                });
            }
        }

        $('.bart-perk-custom-form .custom-form-submit').attr('disabled', true);
        // Submit Button Enable when field has value
        $('.bart-perk-custom-form input').keyup(function () {
            custom_enable_submit_btn('.bart-perk-custom-form');
        });
        // Submit Button Enable when field has value
        $('.gift-card-login input').keyup(function () {
            custom_enable_submit_btn('.gift-card-login');
        });
        // Redeem Page Image Popup
        $(".page-user-redemption .redem-img").each(function () {
            $(this).click(function () {
                $(this).colorbox({
                    width: "60%",
                    height: "300px",
                    inline: true,
                    href: $(".redeem-popup", this)
                });
            });
            //Redeem Thank you Popup
            $(this).find('.redeem-rwd-btn').click(function () {
                $(this).colorbox({
                    width: "60%",
                    height: "300px",
                    inline: true,
                    href: ".redeem-thankyou-popup"
                });
            });
        });
        //Hide Popup
        $(".colorbox-close-btn").click(function () {
            $(this).colorbox.close();
        });
        var path = window.location.pathname;
        $('.history-page-links a[href="' + path + '"]').addClass('active');


        function custom_enable_submit_btn(formClass) {
            var val_empty = false;
            // Add class for Filled value fields
            $(formClass + ' input').each(function () {
                console.log($(this).parent());
                if (!$(this).val()) {
                    val_empty = true;
                    if ($(this).hasClass("field-val-filled") && !$(this).hasClass("cus-remember-me")) {
                        $(this).removeClass("field-val-filled");
                        $(this).parent('.form-item').removeClass("filled-val-parent");
                    }
                } else {
                    if (!$(this).hasClass("field-val-filled") && !$(this).hasClass("cus-remember-me")) {
                        $(this).addClass('field-val-filled');
                        $(this).parent('.form-item').addClass("filled-val-parent");
                    }
                }
            });
            if (val_empty) {
                $(formClass + ' .custom-form-submit').attr('disabled', true);
                if (!$(formClass + ' .custom-form-submit').hasClass("cus-btn-disabled")) {
                    $(formClass + ' .custom-form-submit').addClass("cus-btn-disabled");
                }
            } else {
                $(formClass + ' .custom-form-submit').attr('disabled', false);
                $(formClass + ' .custom-form-submit').removeClass("cus-btn-disabled");
            }
        }
        $('.login-wrap .gc-signin-btn').on("click", function () {
            $(this).addClass('open');
            $('.signup-wrap .gc-signup-btn').removeClass('open');
            $('.login-wrap form').slideToggle();
            $('.signup-wrap form').slideUp();
        });
        $('.signup-wrap .gc-signup-btn').on("click", function () {
            $(this).addClass('open');
            $('.login-wrap .gc-signin-btn').removeClass('open');
            $('.signup-wrap form').slideToggle();
            $('.login-wrap form').slideUp();
        });
        function GetQueryStringParams(sParam) {
            var sPageURL = window.location.search.substring(1);
            var sURLVariables = sPageURL.split('&');
            for (var i = 0; i < sURLVariables.length; i++) {
                var sParameterName = sURLVariables[i].split('=');
                if (sParameterName[0] == sParam) {
                    return sParameterName[1];
                }
            }
        }
        // Point/Trip History & Offer History Filter Changes
        $(".point-range").hide();
        var min_date = GetQueryStringParams('min_points');
        var max_date = GetQueryStringParams('max_points');
        if (min_date !== undefined && max_date !== undefined) {
            $(".point-range").show();
            $('.bart-perk-custom-form .custom-form-submit').attr('disabled', false);
            $('.bart-perk-custom-form .custom-form-submit').removeClass("cus-btn-disabled");
        }
        $(".filter-by-point").click(function () {
            $(".point-range").toggle();
            $(".calender").hide();
        });
        // on click outside close point value filter
        $('body').click(function (evt) {
            if (!$(evt.target).is('.filter-by-point, .filter-by-point *, .point-range, .point-range * ')) {
                //event handling code
                $(".point-range").hide();
            }
        });
        $('body').click(function (evt) {
            if (!$(evt.target).is('.filter-by-date, .filter-by-date *, .calendar, .calendar * ')) {
                //event handling code
                $(".calender").hide();
            }
        });
    });

})(jQuery);
