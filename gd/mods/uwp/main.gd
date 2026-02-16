extends Node

var config_handler = preload("res://mods/uwp/config_handler.tscn")
var menu_button = preload("res://mods/uwp/Scenes/discord_button.tscn")
var option_patches = preload("res://mods/uwp/patches/better-settings-numbers/better-settings-numbers.gd")

var patches := [
	preload("res://mods/uwp/patches/persistent-ignores.gd"),
	preload("res://mods/uwp/patches/unlimit-props.gd"),
	preload("res://mods/uwp/patches/delete-canvas.gd"),
	preload("res://mods/uwp/patches/persistent-bans/persistent-bans.gd"),
]


func _ready() -> void:
	self.add_child(config_handler.instance(), true)
	self.add_child(option_patches.new())
	get_tree().connect("node_added", self, "_join_tree")
	get_tree().connect("node_added", self, "_add_menu_button")


func _load_patches() -> void:
	for patch in patches:
		add_child(patch.new())


func _join_tree(node: Node) -> void:
	var map: Node = get_tree().current_scene
	var in_game = map.name == "world"
	if in_game:
		get_tree().disconnect("node_added", self, "_join_tree")
		self._load_patches()


func _add_menu_button(menu: Node) -> void:
	if menu.name != "main_menu":
		return
	var menu_buttons: Node = menu.get_node("VBoxContainer")

	var button: Button = menu_button.instance()
	button.connect("pressed", self, "_on_button_pressed")

	var settings_button: Node = menu_buttons.get_node("settings")
	menu_buttons.add_child(button)
	menu_buttons.move_child(button, settings_button.get_index())
	menu_buttons.margin_top -= 48


func _on_button_pressed() -> void:
	OS.shell_open("https://discord.gg/kjf3FCAMDb")
