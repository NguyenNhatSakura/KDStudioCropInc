﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class RoundedForm : Form
{
    private int borderRadius = 30; // Adjust the radius value


    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        // Define a GraphicsPath for the rounded rectangle
        GraphicsPath path = new GraphicsPath();
        path.StartFigure();
        path.AddArc(new Rectangle(0, 0, borderRadius, borderRadius), 180, 90); // Top-left corner
        path.AddArc(new Rectangle(this.Width - borderRadius, 0, borderRadius, borderRadius), 270, 90); // Top-right corner
        path.AddArc(new Rectangle(this.Width - borderRadius, this.Height - borderRadius, borderRadius, borderRadius), 0, 90); // Bottom-right corner
        path.AddArc(new Rectangle(0, this.Height - borderRadius, borderRadius, borderRadius), 90, 90); // Bottom-left corner
        path.CloseFigure();

        // Apply the GraphicsPath to the form's region to make the form round
        this.Region = new Region(path);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        // Redraw the form to maintain the round corners on resize
        this.Invalidate();
    }

    // Optional: Allow dragging the borderless form
    protected override void WndProc(ref Message m)
    {
        const int WM_NCHITTEST = 0x84;
        const int HTCLIENT = 1;
        const int HTCAPTION = 2;

        if (m.Msg == WM_NCHITTEST)
        {
            base.WndProc(ref m);
            if ((int)m.Result == HTCLIENT)
            {
                m.Result = (IntPtr)HTCAPTION;
                return;
            }
        }
        base.WndProc(ref m);
    }
}

public class RoundedButton : Button
{
    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);

        // Define a GraphicsPath for the rounded button
        GraphicsPath path = new GraphicsPath();
        path.AddArc(new Rectangle(0, 0, 20, 20), 180, 90); // Top-left corner
        path.AddArc(new Rectangle(this.Width - 21, 0, 20, 20), 270, 90); // Top-right corner
        path.AddArc(new Rectangle(this.Width - 21, this.Height - 21, 20, 20), 0, 90); // Bottom-right corner
        path.AddArc(new Rectangle(0, this.Height - 21, 20, 20), 90, 90); // Bottom-left corner
        path.CloseAllFigures();

        // Apply the GraphicsPath to the button's region to make the button round
        this.Region = new Region(path);
    }
}
