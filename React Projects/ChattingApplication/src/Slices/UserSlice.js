import { createSlice } from "@reduxjs/toolkit";

const initialState = {
    user : {}
}

export const UserSlice = createSlice({
    initialState,
    name: 'userDetails',
    reducers: {
        setUser(state, action){
            state.user = action.payload
        }
    }
});

export const {setUser} = UserSlice.actions;
export default UserSlice.reducer;