import { SET_ISSUES } from '@/actions/IssuesActions';

const initialState = {
  issuesList: [],
};

const issuesReducer = (state = initialState, action) => {
  switch (action.type) {
    case SET_ISSUES:
      return {
        ...state,
        issuesList: action.payload,
      };
    default:
      return state;
  }
};

export default issuesReducer;
