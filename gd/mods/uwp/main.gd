extends Node

## TODO!
var patches := [
	preload("res://mods/uwp/patches/persistent-ignores.gd"),
	preload("res://mods/uwp/patches/unlimit-props.gd")
]


func _ready():
	get_tree().connect("node_added", self, "_join_tree")


func _load_patches():
	for patch in patches:
		add_child(patch.new())


func _join_tree(node):
	var map: Node = get_tree().current_scene
	var in_game = map.name == "world"
	if in_game:
		get_tree().disconnect("node_added", self, "_join_tree")
		self._load_patches()

