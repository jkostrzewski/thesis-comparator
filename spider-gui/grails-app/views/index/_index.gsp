<div class="container">
	<div class="logo">
		<g:img dir="images" file="logo.png" style="width:450px; height:300px"/>
	</div>

	<div class="container">
		<div class="row">
			<div class="col-md-2 col-md-offset-4">
				<button type="button"
					class="btn btn-large btn-block btn-primary full-width"
					onClick="loadResultPage(false, true)">Default search</button>
			</div>
			<div class="col-md-2">
				<button type="button"
					class="btn btn-large btn-block btn-default full-width"
					onClick="loadCustomizePage()">Customize search</button>
			</div>
		</div>
		<div class="row">
			<div class="col-md-12">
				 <p class="text-center orText">or</p>
			</div>
		</div>
		<div class="row">
			<div class="col-md-4 col-md-offset-4 text-center">
				<a	href="#"
					class=""
					onClick="loadResultPage(false, false)">View database</a>
			</div>
		</div>
	</div>
</div>


