// assert(string msg) macro/alias
#DEFINE assert(msg) printf(msg)


// AI State enum
enum ai_state_type{
    none,
    idle,
    patrol,
    chase,
}

// AI waypoint (where to move, how long to stay there)
struct ai_waypoint {
    vec2d position;
    int wait_time_frames;
};

// Struct to hold all information about AI state.
struct ai_state {
    vec2d position;
    ai_state_type active_state;     // Which state AI is currently in.
    int idle_max_wait_frames;       // How long to idle, in frames.
    int idle_rem_wait_frames;       // How many frames remaining for AI to wait.
    int patrol_waypoint_count;      // Number of waypoints.
    int patrol_waypoint_index;      // Current waypoint index.
    float patrol_vision_radius;     // Vision radius of AI when patrolling.
    float patrol_move_speed;        // Speed of AI when patrolling.
    ai_waypoint patrol_waypoints[8];// Waypoints. Maximum of 8.
    float chase_move_speed;         // Speed of AI when chasing player.
    player *chase_player_state;     // Pointer to player, for chasing.
};

// Initialize AI
// CONSIDER: moving some stats into own struct? Allow defining different things for different AI.
void ai_state_init(ai_state *state){
    state->active_state = ai_state_type.none;
    state->idle_max_wait_frames = 30;
    state->idle_rem_wait_frames = 0;
    state->patrol_waypoint_count = 0;
    state->patrol_move_speed = 3f;
    state->patrol_vision_radius = 5f;
    state->patrol_waypoints = {
        { .position = {0,0} },
        { .position = {0,0} },
        { .position = {0,0} },
        { .position = {0,0} },
        { .position = {0,0} },
        { .position = {0,0} },
        { .position = {0,0} },
        { .position = {0,0} },
    };
    state->chase_move_speed = 4f;
    state->chase_player_state = NULL;
}

// Init call for IDLE state
void ai_set_state_idle(ai_state *state){
    // Set state flag
    state->active_state = ai_state_type.idle;
    // Set number of frames AI will idle for.
    state->idle_rem_wait_frames = ai_state->idle_max_wait_frames;
}
// IDLE
void ai_idle(ai_state *state){
    // Sanity check.
    bool is_valid_state = state->active_state == ai_state_type.idle;
    if (!is_valid_state)
        assert("Assert: trying to call state IDLE from another state.");
    // Decrement wait time
    state->idle_rem_wait_frames--;
    // Change state if no longer required to idle.
    // Transition to patrol state
    bool is_done_waiting = state->idle_rem_wait_frames <= 0;
    if (is_done_waiting)
        ai_set_state_patrol(state);
}

// 
void ai_set_state_patrol(ai_state *state){
    // Set state flag
    state.active_state = ai_state_type.patrol;
}
// 
void ai_patrol(ai_state *state){
    // Sanity check.
    bool is_valid_state = state->active_state == ai_state_type.patrol;
    if (!is_valid_state)
        assert("Assert: trying to call state PATROL from another state.");
    // Vision check 
    bool can_see_player = ai_vision_check(state);
    if (can_see_player){
        // Transition to chase state
        ai_set_state_chase(state);
    } else {
        // Get current waypoint
        int waypoint_index = state->patrol_waypoint_index;
        waypoint w = state->waypoints[current];
        // Move AI towards waypoint
        // TODO
    }
}

//
void ai_set_state_chase(ai_state *state){
    // Set state flag
    state->active_state = ai_state_type.chase;
}
// 
void ai_chase(ai_state *state){
    // Sanity check.
    bool is_valid_state = state->active_state == ai_state_type.chase;
    if (!is_valid_state)
        assert("Assert: trying to call state CHASE from another state.");
    bool can_see_player = ai_vision_check(state);
    if (can_see_player){
        // Get direction vector to player
        // TODO
        // Apply vector to position with consideration for collision
        // TODO
    } else {
        // Transition to idle state
        ai_set_state_idle(state);
    }
}

void ai_move(ai_state *state, vec2d direction){
    // TODO
}

bool ai_vision_check(ai_state *state){
    // Sanity check.
    bool has_player_reference = ai_state_type.chase_player_state == NULL;
    if (!has_player_reference)
        assert("Assert: trying to check null PLAYER_STATE in AI vision check.");    
    // Distance check from AI to player (squared, not with square root)
    float vision_radius = state->active_state == ai_state_type.chase
        ? state->vision_radius + 1.0f     // Pad vision so AI doesn't flicker between states
        : state->vision_radius;           // Use regular vision distance
    float max_distance_squared = vision_radius * vision_radius;
    float distance_squared = vec2d_distance_squared(state->position, state->chase_player_state->position);
    bool is_within_range = distance_squared < max_distance_squared;
    if (is_within_range){
        // Do a more refined check (ie: check tiles between AI and player for wall.)
        bool temp_can_see_player = true;
        if (temp_can_see_player)
            return true;
    }
    // Fallback
    return false;
} 

float vec2d_distance_squared(vec2d a, vec2d b){
    vec2d delta = vec2d_sub(a, b);
    float x = delta.x;
    float y = delta.y;
    float distance_squared = x*x + y*y;
    return distance_squared;
}
vec2d vec2d_sub(vec2d a, vec2d b){
    float x = a.x - b.x;
    float y = a.y - b.y;
    vec2d v = {x, y};
    return v;
}







struct player_state{
    vec2d position;
};

struct input_state{

};

// player
void player_move(player_state *player, input_state *input){

}

void player_move_carry(player_state *player, input_state *input){

}

void player_throw(player_state *player, input_state *input){

}

void player_death(player_state *player, input_state *input){

}