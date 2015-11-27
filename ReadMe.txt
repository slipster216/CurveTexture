Curve Texture for Unity
©2015 Jason Booth
	
	If you do a lot of shader work, you might find yourself needing to encode a curve or LUT into a texture. People usually do this with gradients in photoshop, but it’s often difficult to control the curve when you can only view and work with it as a color gradient. Curve texture allows you to use 4 unity curves to generate a single RGBA texture, allowing you to easily encode 4 arbitrary curves into a texture.
	To use the component, add it to any object with a Renderer component; it will look at the shader on the material and give you an option for which texture property to target. If it doesn’t find a renderer, it will give you a text label allowing you to set it as a global shader property. 
	You can select the size of the texture to generate as well as the wrap, filter, and anisotropic properties. Finally, you can save the texture in case you’d prefer to bake it out and not generate it at runtime.
	The main benefit of using the component is iteration speed and workflow; the data adjusts interactively in the editor, rather than having to flip to an external app, adjust, and reimport, and it’s easy to represent exactly the curve you desire. 
	
   Gradient Texture is the same thing, but with a gradient, which is more an ease of use thing since gradients are easy to create in art tools. 