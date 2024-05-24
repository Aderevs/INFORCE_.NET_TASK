import { AllComponent, CertainComponent } from "./components";

export const shortenedUrlsRoutes = [
    { path: 'urls/all', component: AllComponent },
    { path: 'urls/certain/:id', component: CertainComponent }
]