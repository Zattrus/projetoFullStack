import { createBrowserRouter } from "react-router-dom";
import AcceptedPage from "./ui/AcceptedPage";
import App from "./ui/App";
import InvitedPage from "./ui/InvitedPage";
export const router = createBrowserRouter([
  {
    path: "/", element: <App />, children: [
      { index: true, element: <InvitedPage /> },
      { path: "accepted", element: <AcceptedPage /> },
    ]
  },
]);
