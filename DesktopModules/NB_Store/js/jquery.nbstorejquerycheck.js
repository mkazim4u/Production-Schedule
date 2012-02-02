
 jQuery(document).ready(function(){

 if (/1\.(0|1|2|3)\.(0|1|2)/.test(jQuery.fn.jquery) 
                    || /^1.1/.test(jQuery.fn.jquery) 
                    || /^1.2/.test(jQuery.fn.jquery) 
 					)
{
alert('jQuery upgrade needed for NB_Store.  Required version: 1.4.2 Current version: '+ jQuery.fn.jquery);    //Tell user they need to upgrade.
}


});
