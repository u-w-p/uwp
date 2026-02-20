## Makes banning players persistent
extends Node

const DEBUG := false
const BANS_FILE_PATH = "user://bans.txt"
var bans_file: File = File.new()
var banned_players: = []
var timer: Timer


func _debug(msg, data = null):
	if not DEBUG:
		return
	print("[UWP]: %s" % msg)
	if data != null:
		print(JSON.print(data, "\t"))


func _add_ban(id_or_ids):
	var ids: Array
	if not (typeof(id_or_ids) == TYPE_ARRAY):
		ids = [id_or_ids]
	else:
		ids = id_or_ids
	for id in ids:
		id = int(id)
		if not id in Network.WEB_LOBBY_REJECTS:
			Network.WEB_LOBBY_REJECTS.append(id)
			_debug("Added player to lobby rejects", id)
		if not id in banned_players:
			banned_players.append(id)
			if not bans_file.is_open():
				bans_file.open(BANS_FILE_PATH, File.WRITE)
			bans_file.store_line(str(id))
			_debug("Added player to banned list", id)
	if bans_file.is_open():
		bans_file.close()


func _exit_tree() -> void:
	if bans_file.is_open():
		bans_file.close()


func _update_lists() -> void:
	if not _am_lobby_host():
		return
	if Network.WEB_LOBBY_REJECTS.empty():
		return
	_debug("Saving rejects to file...")
	_add_ban(Network.WEB_LOBBY_REJECTS)


func _am_lobby_host() -> bool:
	return Network.STEAM_LOBBY_ID > 0 and Steam.getLobbyOwner(Network.STEAM_LOBBY_ID) == Network.STEAM_ID


func _on_entity_spawn(node: Node):
	if node.name == "player" and _am_lobby_host():
		# Technically this could fire if the host player is cloning
		_debug("Loading ban list into new lobby...")
		for reject in banned_players:
			_debug("Loaded banned player", str(reject))
			Network.WEB_LOBBY_REJECTS.append(reject)
		timer.start()
	if node.name == "main_menu":
		timer.stop()

func _ready():
	get_tree().current_scene.get_node("Viewport/main/entities").connect("child_entered_tree", self, "_on_entity_spawn")
	if not bans_file.file_exists(BANS_FILE_PATH):
		bans_file.open(BANS_FILE_PATH, File.WRITE_READ)
		bans_file.close()
	else:
		var banned_players: = []
		bans_file.open(BANS_FILE_PATH, File.READ)
		while bans_file.get_position() < bans_file.get_len():
			var id = int(bans_file.get_line())
			_debug("Loaded banned player", str(id))
			banned_players.append(id)
		bans_file.close()
		_add_ban(banned_players)

	timer = Timer.new()
	timer.name = "Banned User List Save"
	timer.wait_time = 300
	timer.autostart = false
	timer.connect("timeout", self, "_update_lists")
	add_child(timer)
