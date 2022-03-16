To be able to use this host, first should modify these missing information at app.config file:

	1) replace [your namespace] at this line:
	<add baseAddress="https://[your namespace].servicebus.windows.net/" />

	2) replace [your key] & [your secret] at these lines:
	<sharedAccessSignature keyName="[your key]"
	                       key="[your secret]" />