import { createSlice } from "@reduxjs/toolkit"

const initialState = {
    isUserSelectionEnabled : false,
    isCreateGroupButtonEnabled : false,
    createdGroupObject : {}
}

export const GroupSettingSlice = createSlice({
    initialState,
    name: 'groupSetting',
    reducers: {
        setIsUserSelectionEnabled(state, action){
            state.isUserSelectionEnabled = action.payload
        },
        setIsCreateGroupButtonEnabled(state, action){
            state.isCreateGroupButtonEnabled = action.payload
        },
        setCreatedGroupObject(state, action){
            state.createdGroupObject = action.payload
        }
    }
});

export const {setIsCreateGroupButtonEnabled, setIsUserSelectionEnabled, setCreatedGroupObject} = GroupSettingSlice.actions;
export default GroupSettingSlice.reducer;