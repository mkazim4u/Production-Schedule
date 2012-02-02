
 jQuery(document).ready(function(){

 
    jQuery('.textexp_extra').focus(function() {
        if (jQuery(this).width() <= '200') {
            jQuery('.textexp_name').animate({ 'width': '40px' }, 'slow');
            jQuery('.textexp_code').animate({ 'width': '40px' }, 'slow');
            jQuery('.textexp_extra').animate({ 'width': '280px' }, 'slow');
            return false;
        }
    });
    jQuery('.textexp_name').focus(function() {
        if (jQuery(this).width() <= '200') {
            jQuery('.textexp_code').animate({ 'width': '40px' }, 'slow');
            jQuery('.textexp_extra').animate({ 'width': '40px' }, 'slow');
            jQuery('.textexp_name').animate({ 'width': '280px' }, 'slow');
            return false;
        }
    });
    jQuery('.textexp_code').focus(function() {
        if (jQuery(this).width() <= '200') {
            jQuery('.textexp_extra').animate({ 'width': '40px' }, 'slow');
            jQuery('.textexp_name').animate({ 'width': '40px' }, 'slow');
            jQuery('.textexp_code').animate({ 'width': '280px' }, 'slow');
            return false;
        }
    });


    var COOKIE_NAME = 'nb_store_admintab';
    var options = { path: '/', expires: 1 };
    var activetab = jQuery.cookie(COOKIE_NAME)

    if (activetab == null) {
        activetab = '#tabs-1'
        jQuery.cookie(COOKIE_NAME, activetab, options);
    };

    hidedivs();

    if (jQuery('#buttontabs-1').attr("href") == undefined) {
        activetab = '#tabs-8'
    };

    jQuery(activetab).show();

    if (activetab == '#tabs-8') {
        jQuery('#tabs-1').show();
        jQuery('#tabs-2').show();
        jQuery('#tabs-3').show();
        jQuery('#tabs-4').show();
    };

	
    jQuery('#buttontabs-1').click(
    function() {
        showdiv('#tabs-1');
        return false;
    });
    jQuery('#buttontabs-2').click(
    function() {
        showdiv('#tabs-2');
        return false;
    });
    jQuery('#buttontabs-3').click(
    function() {
        showdiv('#tabs-3');
        return false;
    });
    jQuery('#buttontabs-4').click(
    function() {
        showdiv('#tabs-4');
        return false;
    });
    jQuery('#buttontabs-5').click(
    function() {
        showdiv('#tabs-5');
        return false;
    });
    jQuery('#buttontabs-6').click(
        function() {
        showdiv('#tabs-6');
        return false;
    });
    jQuery('#buttontabs-7').click(
        function() {
        showdiv('#tabs-7');
        return false;
    });
    jQuery('#buttontabs-8').click(
        function() {
		jQuery(jQuery.cookie(COOKIE_NAME)).hide();
		jQuery.cookie(COOKIE_NAME, '#tabs-8', options);
        jQuery('#tabs-1').show();
        jQuery('#tabs-2').show();
        jQuery('#tabs-3').show();
        jQuery('#tabs-4').show();
        return false;
    });

    function hidedivs() {
        jQuery('#tabs-1').hide();
        jQuery('#tabs-2').hide();
        jQuery('#tabs-3').hide();
        jQuery('#tabs-4').hide();
        jQuery('#tabs-5').hide();
        jQuery('#tabs-6').hide();
        jQuery('#tabs-7').hide();
        jQuery('#tabs-8').hide();
    };

    function showdiv(divid) {
	    hidedivs();
        jQuery(divid).show(0, function() {
            jQuery.cookie(COOKIE_NAME, divid, options);
            return false;
        });

    };
		

});
