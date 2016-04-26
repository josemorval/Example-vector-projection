using UnityEngine;
using System.Collections;

public class ProjRenderer : MonoBehaviour {

	[Range(0.1f,0.5f)]
	public float radius;
	public float angle1;
	public float angle2;

	Vector3 pos;

	public Transform spherePos;
	public Transform spherePosProjX;
	public Transform spherePosProjZ;

	public Material matTri;
	public Material matLine;

	public Material matTriProjX;
	public Material matLineProjX;

	public Material matTriProjZ;
	public Material matLineProjZ;

	public float separationWidth;

	Mesh triMesh;
	Mesh lineMesh;

	Mesh triMeshProjX;
	Mesh lineMeshProjX;

	Mesh triMeshProjZ;
	Mesh lineMeshProjZ;

	Vector3[] verts;
	int[] inds;

	Vector3 posProj;
	Vector3 posProjX;
	Vector3 posProjZ;

	void Start () {

		triMesh = new Mesh();
		lineMesh = new Mesh();

		triMeshProjX = new Mesh();
		lineMeshProjX = new Mesh();

		triMeshProjZ = new Mesh();
		lineMeshProjZ = new Mesh();
	}
	
	void Update () {
	

		InputControl();

		pos = radius*new Vector3(
			Mathf.Cos(2f*Mathf.PI*angle1)*Mathf.Cos(2f*Mathf.PI*angle2),
			Mathf.Sin(2f*Mathf.PI*angle2),
			Mathf.Sin(2f*Mathf.PI*angle1)*Mathf.Cos(2f*Mathf.PI*angle2)
		);

		posProjX = Vector3.Dot(pos,transform.right) * transform.right;
		posProjZ = Vector3.Dot(pos,transform.forward) * transform.forward;
		posProj = posProjX + posProjZ;

		RenderCenterZone();
		RenderProjXZone();
		RenderProjZZone();

		spherePos.position = pos;
		spherePosProjX.position = posProjX;
		spherePosProjZ.position = posProjZ;

	}

	void InputControl(){

		if(Input.GetKey(KeyCode.A)){
			radius -= 0.15f*Time.deltaTime;
		}else if(Input.GetKey(KeyCode.Q)){
			radius += 0.15f*Time.deltaTime;
		}

		radius = Mathf.Clamp(radius,0.1f,0.5f);

		if(Input.GetKey(KeyCode.S)){
			angle1 -= 0.15f*Time.deltaTime;
		}else if(Input.GetKey(KeyCode.W)){
			angle1 += 0.15f*Time.deltaTime;
		}

		angle1%=2f*Mathf.PI;

		if(Input.GetKey(KeyCode.D)){
			angle2 -= 0.15f*Time.deltaTime;
		}else if(Input.GetKey(KeyCode.E)){
			angle2 += 0.15f*Time.deltaTime;
		}

		angle2%=2f*Mathf.PI;


	}

	void RenderCenterZone(){

		verts = new Vector3[3];
		verts[0] = Vector3.zero;
		verts[1] = pos;
		verts[2] = posProj;

		inds = new int[]{0,1,2};

		triMesh.vertices = verts;
		triMesh.SetIndices(inds,MeshTopology.Triangles,0);
		triMesh.Optimize();

		Graphics.DrawMesh(triMesh,transform.localToWorldMatrix,matTri,0);

		inds = new int[]{0,1,2,0};
		lineMesh.vertices = verts;
		lineMesh.SetIndices(inds,MeshTopology.LineStrip,0);
		lineMesh.Optimize();

		Graphics.DrawMesh(lineMesh,transform.localToWorldMatrix,matLine,0);

	}

	void RenderProjXZone(){

		verts = new Vector3[3];
		verts[0] = Vector3.zero+separationWidth*Mathf.Sign(posProjX.x) * transform.right;
		verts[1] = posProjX;
		verts[2] = posProj - separationWidth*Mathf.Sign(posProjZ.z) * transform.forward;

		inds = new int[]{0,1,2};

		triMeshProjX.vertices = verts;
		triMeshProjX.SetIndices(inds,MeshTopology.Triangles,0);
		triMeshProjX.Optimize();

		Graphics.DrawMesh(triMeshProjX,transform.localToWorldMatrix,matTriProjX,0);

		inds = new int[]{0,1,2,0};
		lineMeshProjX.vertices = verts;
		lineMeshProjX.SetIndices(inds,MeshTopology.LineStrip,0);
		lineMeshProjX.Optimize();

		Graphics.DrawMesh(lineMeshProjX,transform.localToWorldMatrix,matLineProjX,0);

	}

	void RenderProjZZone(){

		verts = new Vector3[3];
		verts[0] = Vector3.zero+separationWidth*Mathf.Sign(posProjZ.z) * transform.forward;
		verts[1] = posProjZ;
		verts[2] = posProj - separationWidth*Mathf.Sign(posProjX.x) * transform.right;

		inds = new int[]{0,1,2};

		triMeshProjZ.vertices = verts;
		triMeshProjZ.SetIndices(inds,MeshTopology.Triangles,0);
		triMeshProjZ.Optimize();

		Graphics.DrawMesh(triMeshProjZ,transform.localToWorldMatrix,matTriProjZ,0);

		inds = new int[]{0,1,2,0};
		lineMeshProjZ.vertices = verts;
		lineMeshProjZ.SetIndices(inds,MeshTopology.LineStrip,0);
		lineMeshProjZ.Optimize();

		Graphics.DrawMesh(lineMeshProjZ,transform.localToWorldMatrix,matLineProjZ,0);
	}
}
