import prisma from "@/prisma/client";
import { NextRequest } from "next/server";

export async function PUT(request: NextRequest, { params }: { params: { id: string } }){

    const { id } = params;
    const { status } = await request.json();

}