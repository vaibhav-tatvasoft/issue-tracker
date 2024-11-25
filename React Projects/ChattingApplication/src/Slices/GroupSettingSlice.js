import { createSlice } from "@reduxjs/toolkit"

const initialState = {
    isUserSelectionEnabled : false,
    isCreateGroupButtonEnabled : false
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
        }
    }
});

export const {setIsCreateGroupButtonEnabled, setIsUserSelectionEnabled} = GroupSettingSlice.actions;
export default GroupSettingSlice.reducer;