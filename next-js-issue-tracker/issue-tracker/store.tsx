import { createStore, combineReducers } from 'redux';
import issuesReducer from '@/reducers/IssueReducer';

const rootReducer = combineReducers({
  issues: issuesReducer,
});

const store = createStore(rootReducer);

export default store;
export type RootState = ReturnType<typeof store.getState>;
