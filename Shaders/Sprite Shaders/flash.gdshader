shader_type canvas_item;

uniform vec4 solid_color : source_color = vec4(1,1,1,0);
uniform float amount = 0.25;

void fragment() {
    vec4 texture_color = texture(TEXTURE, UV);
    COLOR = vec4(mix(texture_color.rgb, solid_color.rgb, solid_color.a * amount), texture_color.a);
}