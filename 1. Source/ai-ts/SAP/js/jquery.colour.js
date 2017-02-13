jQuery.fn.colour =
	function(styles, options)
	{
		var _context = this;
		this.colour = new Function();
		
		function makeMakeRedFunction(context)
		{
			return function()
			{
				$(context).css("color", "#f00");
			};
		}
		
		function makeMakeBlueFunction(context)
		{
			return function()
			{
				$(context).css("color", "#00f");
			};
		}
		
		function makeInvertFunction(context)
		{
			return function(force)
			{
				invert(context[0].internalColourState, force);
			}
		}
		
		function invert(colourState, force)
		{
			var elementJQ = colourState.getElementJQ();
			var colour = elementJQ.css("color");
			var newColour;
			if (colour === "rgb(255, 0, 0)")
			{
				newColour = "#00f";
			}
			else
			{
				newColour = "#f00";
			}
			if (force)
			{
				newColour = "#0f0";
			}
			
			elementJQ.css("color", newColour);
		}
		
		jQuery.fn.extend(
			this.colour,
			{
				makeRed: makeMakeRedFunction(_context),
				makeBlue: makeMakeBlueFunction(_context),
				invert: makeInvertFunction(_context)
			});
		
		// Each element is checked
		return this.each(
			function()
			{
				this.internalColourState = new ColourState(this);
				
				function ColourState(domElement)
				{
					var _elementJQ = $(domElement);
					
					this.getElementJQ =
						function()
						{
							return _elementJQ;
						}
						
					this.initialise =
						function()
						{
							_elementJQ.css("color", "#000");		
						}
				}
				
				this.internalColourState.initialise();
			});
	}