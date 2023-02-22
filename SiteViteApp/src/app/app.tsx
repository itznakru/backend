import "./app.css";
import { Provider } from "react-redux";
import store from "./store";
import { createBrowserRouter, RouterProvider } from "react-router-dom"
import Wizard from "../wizard/wizard.tsx"
import '../fontawesome-free-6.3.0-web/css/all.min.css'

const router = createBrowserRouter([
  {
    path: "/",
    element: <div>root</div>,
    errorElement: <h3>ERROR PAGE</h3>,
  },
  {
    path: "/wizard",
    element: <Wizard></Wizard>,
  },
  {
    path: "/archive",
    element: <h3>ARHIVE</h3>,
  }
  
]);

function App() {
  return (
    <div className="app__container">      
        <RouterProvider router={router} />      
    </div>
  );
}

export default App;
