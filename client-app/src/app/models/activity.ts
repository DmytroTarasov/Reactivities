export interface Activity {
    id: string;
    title: string;
    date: Date | null; // can be of type Date or null
    description: string;
    category: string;
    city: string;
    venue: string;
}