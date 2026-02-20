## Prevents chalk from being drawn outside of the canvas

extends Node

const DEBUG := false

## Cached local player ref
var _local_player


func _debug(msg, data = null) -> void:
	if not DEBUG:
		return
	print("[UWP]: %s" % msg)
	if data != null:
		print(JSON.print(data, "\t"))


## Borrowed from Chalk++
## https://github.com/binury/Toes.ChalkPlusPlus/blob/main/gdscripts/mods/Toes.ChalkPlusPlus/controls.gd#L636
##
## Full of edge cases and exceptions but the gist is whether it's inside the visual circle
## which is not necessarily where the actual grid node is... because LameDev
func in_bounds(x: int, y: int, chalk_canvas_node: Spatial) -> bool:
	var canvas_id: int = chalk_canvas_node.canvas_id
	# Disregard non-standard canvasses - unable to be determined
	if canvas_id > 3:
		return true
	## The small canvas
	var should_use_mini_hack = canvas_id == 1
	## The uphill canvasses are not properly algned either
	var should_use_offset_hack = canvas_id != 0 and not should_use_mini_hack
	# Yes, in summary, only the main spawn canvas is centered properly

	# Gross edge case in how I use the canvas_boundry property
	# to calculate the circle's area, in part due to significant misalignment of texture
	var width = (100.0 if should_use_mini_hack else chalk_canvas_node.canvas_boundry) * 2
	# Quick and dirty hack to hide misalignment of map by enlarging circle slightly
	if should_use_offset_hack:
		width += 11

	var height = width

	var center_x = width / 2.0
	var center_y = height / 2.0
	var radius = width / 2.0
	if should_use_mini_hack:
		# 'Mini' canvas
		center_x /= 2.0
		radius /= 2.0
		# Accounting for significant misalignment; it is NOT centered
		center_x += 50
	var radius_sq = radius * radius
	if x < 0 or x >= width or y < 0 or y >= height:
		return false

	var dx = x - center_x
	var dy = y - center_y
	return (dx * dx + dy * dy) <= radius_sq


func _init():
	self.name = "cleanerCanvasses"
