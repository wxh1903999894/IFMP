$(document).ready(function () {
	/*横屏变为2列*/
    function pad() {
		if (window.orientation == 0 || window.orientation == 180) {
                    $("body").removeClass("hpad");
                    
                } else if (window.orientation == 90 || window.orientation == -90) {
                    // 横屏
					$("body").addClass("hpad");
                }
  	};
	
	pad();
	
	$(window).bind('orientationchange', function (e) { pad() })
	
})