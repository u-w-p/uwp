## Fixes muting/blocking players being temporary
##
## Also, chat will adopt your Steam friends choices
extends Node

const DEBUG := false
const IGNORES_FILE_PATH = "user://ignores.sav"
var Ignores_File: File = File.new()
var ignored_players = {}


func _debug(msg, data = null):
	if not DEBUG:
		return
	print("[UWP]: %s" % msg)
	if data != null:
		print(JSON.print(data, "\t"))


func _add_ignore(id_or_ids):
	var ids: Array
	if not (typeof(id_or_ids) == TYPE_ARRAY):
		ids = [id_or_ids]
	else:
		ids = id_or_ids
	for id in ids:
		id = int(id)
		ignored_players[id] = true
		if not id in PlayerData.players_hidden:
			PlayerData.players_hidden.append(id)
		_debug("Added new player to ignore list", id)
	if not Ignores_File.is_open():
		Ignores_File.open(IGNORES_FILE_PATH, File.WRITE)
	Ignores_File.seek(0)
	Ignores_File.store_string(JSON.print(ignored_players))
	Ignores_File.close()


func _get_ignored_steam_users() -> void:
	var ignored_users := []
	for i in range(Steam.getFriendCount(Steam.FRIEND_FLAG_BLOCKED | Steam.FRIEND_FLAG_IGNORED)):
		yield (get_tree().create_timer(0.4), "timeout")
		var steam_id = Steam.getFriendByIndex(i, Steam.FRIEND_FLAG_BLOCKED | Steam.FRIEND_FLAG_IGNORED)
		ignored_users.append(steam_id)
	_debug("ignored users", ignored_users)
	self._add_ignore(ignored_users)

func _exit_tree() -> void:
	if Ignores_File.is_open(): Ignores_File.close()

func _update_lists() -> void:
	_debug("Updating ignored players list...")
	_get_ignored_steam_users()

func _is_player_ignored(player):
	return Steam.getFriendRelationship(player.owner_id) in [1, 5]

func _on_entity_spawn(node: Node):
	if node.name.begins_with("@player@"):
		if _is_player_ignored(node):
			_add_ignore(node.owner_id)

func _ready():
	get_tree().current_scene.get_node("Viewport/main/entities").connect("child_entered_tree", self, "'_on_entity_spawn'")
	if not Ignores_File.file_exists(IGNORES_FILE_PATH):
		Ignores_File.open(IGNORES_FILE_PATH,File.WRITE_READ)
		Ignores_File.store_string('{}')
		Ignores_File.close()
	Ignores_File.open(IGNORES_FILE_PATH, File.READ)
	var content = JSON.parse(Ignores_File.get_as_text())
	Ignores_File.close()
	if not content:
		content = {}
	else:
		content = content.result
	self._add_ignore(content.keys())
	_debug("Initialized", ignored_players)

	var timer = Timer.new()
	timer.name = "SteamNetwork Ignored User Sync"
	timer.wait_time = 600
	timer.autostart = true
	timer.connect("timeout", self, "_update_lists")
	add_child(timer)
