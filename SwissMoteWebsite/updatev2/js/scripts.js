(function($){
	$(document).ready(function() {	

		$('#my-navigation').iptOffCanvas({
		 	baseClass: 'offcanvas',
		 	type: 'left',
			single: true,			
		 	static: false				  
		});

		//accordian
		 $('.ziehharmonika').ziehharmonika({
			collapsible: true,
			
		});

		jQuery(".team-innertbl a").click(function() {
			jQuery(".popup-area").show()
		})

		jQuery(".cross-bar button").click(function() {
			jQuery(".popup-area").hide()
		})





		
	});
})(jQuery);