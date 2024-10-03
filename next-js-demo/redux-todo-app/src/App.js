import Todo from './components/Todo';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import Users from './components/Users';

 const queryClient = new QueryClient();

function App() {
  return (
    <QueryClientProvider client= {queryClient}>
      <div className = 'child-div'>
        <Users></Users>
      </div>
    </QueryClientProvider>
  );
}

export default App;
