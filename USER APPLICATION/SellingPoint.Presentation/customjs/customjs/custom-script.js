
// material switch button 
$(document).ready(function(e) {
			$('input').lc_switch();

			// triggered each time a field changes status
			$('body').delegate('.lcs_check', 'lcs-statuschange', function() {
				var status = ($(this).is(':checked')) ? 'checked' : 'unchecked';
				console.log('field changed status: '+ status );
			});
			
			
			// triggered each time a field is checked
			$('body').delegate('.lcs_check', 'lcs-on', function() {
				console.log('field is checked');
			});
			
			
			// triggered each time a is unchecked
			$('body').delegate('.lcs_check', 'lcs-off', function() {
				console.log('field is unchecked');
			});
		});