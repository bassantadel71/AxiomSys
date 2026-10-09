console.log('Delivery terms JS loaded');

$(document).ready(function () {

	var texts = document.getElementById('pageTexts').dataset;

	var Toast = Swal.mixin({
		toast: true,
		position: 'top-end',
		showConfirmButton: false,
		timer: 3500,
		timerProgressBar: true
	});

	// 1. Toast messages (shown after the redirect)
	var flash = document.getElementById('flash').dataset;
	if (flash.success) {
		Toast.fire({ icon: 'success', title: flash.success });
	}
	if (flash.error) {
		Toast.fire({ icon: 'error', title: flash.error });
	}

	// 2. Name checks: Arabic field = Arabic letters only, English field = English letters only
	function checkNameField(input) {
		var value = input.value;
		var message = '';

		if ($(input).hasClass('ar-name')) {
			var hasEnglish = /[A-Za-z]/.test(value);
			var hasArabic = /[\u0600-\u06FF]/.test(value);
			if (hasEnglish || (value.trim() !== '' && !hasArabic)) {
				message = texts.arOnly;
			}
		} else {
			var hasArabicLetter = /[\u0600-\u06FF]/.test(value);
			var hasEnglishLetter = /[A-Za-z]/.test(value);
			if (hasArabicLetter || (value.trim() !== '' && !hasEnglishLetter)) {
				message = texts.enOnly;
			}
		}

		input.setCustomValidity(message);
	}

	$(document).on('input', '.ar-name, .en-name', function () {
		checkNameField(this);
		this.reportValidity();
	});

	// 3. DataTable, with the search box moved into the toolbar
	$('#dataTable').DataTable({
	bFilter: true,
	sDom: 'ft',
	paging: false,
	info: false,
	language: {
		search: ' ',
		searchPlaceholder: texts.search
	},
	initComplete: function () {
		$('.dataTables_filter').appendTo(document.getElementById('tableSearch'));
	}
});

	// 4. Edit: load the row through the AJAX handler, then fill the modal
	$(document).on('click', '.edit-btn', function () {
		var code = $(this).data('code');
		var url = window.location.pathname + '?handler=Details&code=' + code;

		fetch(url, { credentials: 'same-origin' })
			.then(function (response) {
				if (response.status === 401) {
					window.location.reload();
					return null;
				}
				if (!response.ok) {
					throw new Error('Request failed: ' + response.status);
				}
				return response.json();
			})
			.then(function (data) {
				if (data === null) {
					return;
				}

				$('#editCode').val(data.code);
				$('#editSName').val(data.sName);
				$('#editBName').val(data.bName);
				$('#editDays').val(data.days);
				$('#editActive').val(data.activeFlag);

				checkNameField(document.getElementById('editSName'));
				checkNameField(document.getElementById('editBName'));

				var modalElement = document.getElementById('editModal');
				var modal = bootstrap.Modal.getOrCreateInstance(modalElement);
				modal.show();
			})
			.catch(function (error) {
				console.error(error);
				Toast.fire({ icon: 'error', title: texts.loadError });
			});
	});

	// 5. Delete: ask first, then submit the hidden form
	$(document).on('click', '.delete-btn', function () {
		var code = $(this).data('code');

		Swal.fire({
			title: texts.confirmTitle,
			text: texts.confirmText,
			icon: 'warning',
			showCancelButton: true,
			confirmButtonText: texts.confirmYes,
			cancelButtonText: texts.cancel
		}).then(function (result) {
			if (result.isConfirmed) {
				$('#deleteCode').val(code);
				$('#deleteForm').submit();
			}
		});
	});
});