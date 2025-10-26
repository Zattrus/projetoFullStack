import { apiClient } from "./client";

export type LeadInvite = {
  id: string;
  contactFirstName: string;
  createdAt: string;
  suburb: string;
  category: string;
  description: string;
  price: number;
};

export type LeadAccepeted = {
  id: string;
  contactFullName: string;
  contactPhone?: string | null;
  contactEmail?: string | null;
  suburb: string;
  category: string;
  description: string;
  price: number;
  createdAt: string;
};

export const fetchInvitedLeads = async (): Promise<LeadInvite[]> => {
  const r = await apiClient.get("/leads", {params: {status: 0}});
  return r.data;
};
export const fetchAcceptedLeads = async (): Promise<LeadAccepeted[]> => {
  const r = await apiClient.get("leads", {params: {status: 1}});
  return r.data;
};

export const acceptLead = (id: string) => apiClient.post(`/leads/${id}/accept`);
export const declineLead = (id: string) => apiClient.post(`/leads/${id}/decline`);